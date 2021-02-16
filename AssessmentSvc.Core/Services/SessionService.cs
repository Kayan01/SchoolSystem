using AssessmentSvc.Core.Interfaces;
using AssessmentSvc.Core.Models;
using AssessmentSvc.Core.ViewModels.SessionSetup;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssessmentSvc.Core.Services
{
    public class SessionService : ISessionSetup
    {
        public readonly IRepository<SessionSetup, long> _sessionRepo;

        private readonly IUnitOfWork _unitOfWork;
        public SessionService(
            IRepository<SessionSetup, long> sessionRepo,
             IUnitOfWork unitOfWork)
        {
            _sessionRepo = sessionRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultModel<SessionSetupDetail>> AddSchoolSession(AddSessionSetupVM vM)
        {
            var result = new ResultModel<SessionSetupDetail>();
            
            var terms =  vM.Terms.Select(x =>
            new Term {
                EndDate = x.EndDate,
                Name = x.Name,
                SequenceNumber = x.SequenceNumber,
                StartDate = x.StartDate
            }).ToList();

            var session = new SessionSetup
            {
                IsCurrent = vM.IsCurrent,
                SessionName = vM.SessionName,
                Terms = terms
            };

            if (vM.IsCurrent == true)
            {
                //update the current session to false
                var lastCurrentSession = await _sessionRepo.GetAll().Where(x => x.IsCurrent).FirstOrDefaultAsync();
                if (lastCurrentSession != null)
                {
                    lastCurrentSession.IsCurrent = false;
                }
            }

            await _sessionRepo.InsertAsync(session);

           await _unitOfWork.SaveChangesAsync();

            result.Data = new SessionSetupDetail
            {
                Id = session.Id,
                Name = session.SessionName,
                IsCurrent = session.IsCurrent,
                Terms = session.Terms.Select(x => new TermVM {
                    EndDate = x.EndDate,
                    Name = x.Name, 
                    SequenceNumber = x.SequenceNumber, 
                    StartDate = x.StartDate })
                .ToList()
            };

            return result;

        }

        public async Task<ResultModel<SessionSetupDetail>> GetCurrentSchoolSession()
        {
            var result = new ResultModel<SessionSetupDetail>();

            var session = await _sessionRepo.GetAll().Where(x => x.IsCurrent == true).FirstOrDefaultAsync();

            if (session == null)
            {
                result.AddError("No current session exists");
                return result;
            }

            result.Data = new SessionSetupDetail
            {
                Id = session.Id,
                IsCurrent = session.IsCurrent,
                Name = session.SessionName,
                Terms = session.Terms.Select(x => new TermVM
                {
                    EndDate = x.EndDate,
                    Name = x.Name,
                    SequenceNumber = x.SequenceNumber,
                    StartDate = x.StartDate
                })
                .ToList()
            };

            if (result.Data.Terms.Count == 1)
            {
                result.Data.Terms[0].IsCurrent = true;
            }
            else if(result.Data.Terms.Count > 1)
            {
                var todayDate = DateTime.Now.Date;
                var curFound = false;

                for (int i = 1; i < result.Data.Terms.Count; i++)
                {
                    if (result.Data.Terms[i-1].StartDate.Date <= todayDate && result.Data.Terms[i].StartDate.Date > todayDate)
                    {
                        result.Data.Terms[i-1].IsCurrent = true;
                        curFound = true;
                    }
                }

                if (!curFound)
                {
                    result.Data.Terms[result.Data.Terms.Count - 1].IsCurrent = true;
                }
            }

            return result;
        }

        public async Task<ResultModel<CurrentSessionAndTermVM>> GetCurrentSessionAndTerm()
        {
            var result = new ResultModel<CurrentSessionAndTermVM>();

            var session = await _sessionRepo.GetAll().Where(x => x.IsCurrent == true).FirstOrDefaultAsync();

            if (session == null)
            {
                result.AddError("No current session exists");
                return result;
            }
            Term currentTerm = new Term();

            if (session.Terms.Count == 1)
            {
                currentTerm = session.Terms[0];
            }
            else if (session.Terms.Count > 1)
            {
                var todayDate = DateTime.Now.Date;

                for (int i = 1; i < session.Terms.Count; i++)
                {
                    if (session.Terms[i - 1].StartDate.Date <= todayDate && session.Terms[i].StartDate.Date > todayDate)
                    {
                        currentTerm = session.Terms[i - 1];
                        break;
                    }
                }

                if (currentTerm?.SequenceNumber is null)
                {
                    currentTerm = session.Terms[session.Terms.Count - 1];
                }
            }

            result.Data = new CurrentSessionAndTermVM
            {
                sessionId = session.Id,
                SessionName = session.SessionName,
                TermName=currentTerm?.Name,
                TermSequence=currentTerm?.SequenceNumber ?? 0,
            };

            return result;
        }

        public async Task<ResultModel<CurrentSessionAndTermVM>> GetSessionAndTerm(long sessionId, int termSequenceNumber)
        {
            var result = new ResultModel<CurrentSessionAndTermVM>();

            var session = await _sessionRepo.GetAll().Where(x => x.Id == sessionId).FirstOrDefaultAsync();

            if (session == null)
            {
                result.AddError("No current session exists");
                return result;
            }
            Term currentTerm = session.Terms.FirstOrDefault(m=>m.SequenceNumber == termSequenceNumber);

            result.Data = new CurrentSessionAndTermVM
            {
                sessionId = session.Id,
                SessionName = session.SessionName,
                TermName = currentTerm?.Name,
                TermSequence = currentTerm?.SequenceNumber ?? 0,
            };

            return result;
        }

        public async Task<ResultModel<List<SessionSetupList>>> GetSchoolSessions()
        {
            var result = new ResultModel<List<SessionSetupList>>();

            var sessions = await _sessionRepo.GetAll().ToListAsync();

            if (sessions == null || sessions.Count < 1)
            {
                result.AddError("No session exists");
                return result;
            }

            result.Data = sessions.Select(x => new SessionSetupList
            {
                Id = x.Id,
                IsCurrent = x.IsCurrent,
                Name = x.SessionName,
                Terms = x.Terms.Select(x => new TermVM
                {
                    EndDate = x.EndDate,
                    Name = x.Name,
                    SequenceNumber = x.SequenceNumber,
                    StartDate = x.StartDate
                })
                .ToList()
            }).ToList();

            return result;

        }

        public async Task<ResultModel<SessionSetupDetail>> UpdateSchoolSession(long Id, AddSessionSetupVM vM)
        {
            var result = new ResultModel<SessionSetupDetail>();

            var session = await _sessionRepo.GetAll().Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (session == null)
            {
                result.AddError("No current session exists");
                return result;
            }

            session.IsCurrent = vM.IsCurrent;
            session.SessionName = vM.SessionName;
            session.Terms = vM.Terms.Select(x => new Term
            {
                EndDate = x.EndDate,
                Name = x.Name,
                SequenceNumber = x.SequenceNumber,
                StartDate = x.StartDate
            }).ToList();


           await _unitOfWork.SaveChangesAsync();

            result.Data = new SessionSetupDetail
            {
                Id = session.Id,
                IsCurrent = session.IsCurrent,
                Name = session.SessionName,
                Terms = session.Terms.Select(x => new TermVM
                {
                    EndDate = x.EndDate,
                    Name = x.Name,
                    SequenceNumber = x.SequenceNumber,
                    StartDate = x.StartDate
                })
                .ToList()
            };
            return result;
        }
    }
}
