using FinanceSvc.Core.Models;
using FinanceSvc.Core.Services.Interfaces;
using FinanceSvc.Core.ViewModels.SessionSetup;
using Microsoft.EntityFrameworkCore;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSvc.Core.Services
{
    public class SessionSetupService : ISessionSetupService
    {
        private readonly IRepository<SessionSetup, long> _sessionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public SessionSetupService(IUnitOfWork unitOfWork, IRepository<SessionSetup, long> sessionRepo)
        {
            _unitOfWork = unitOfWork;
            _sessionRepo = sessionRepo;
        }

        public void AddOrUpdateSessionFromBroadcast(SessionSharedModel model)
        {
            if (model.IsCurrent)
            {
                //update the current session to false
                var lastCurrentSession = _sessionRepo.GetAll().Where(x => x.IsCurrent).FirstOrDefault();
                if (lastCurrentSession != null)
                {
                    lastCurrentSession.IsCurrent = false;
                    _unitOfWork.SaveChanges();
                }
            }

            var session = _sessionRepo.FirstOrDefault(x => x.Id == model.Id);
            if (session == null)
            {
                session = _sessionRepo.Insert(new SessionSetup
                {
                    Id = model.Id
                });
            }

            session.IsCurrent = model.IsCurrent;
            session.SessionName = model.SessionName;
            session.TermsJSON = model.TermsJSON;

            _unitOfWork.SaveChanges();
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
                TermName = currentTerm?.Name,
                TermSequence = currentTerm?.SequenceNumber ?? 0,
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
            Term currentTerm = session.Terms.FirstOrDefault(m => m.SequenceNumber == termSequenceNumber);

            result.Data = new CurrentSessionAndTermVM
            {
                sessionId = session.Id,
                SessionName = session.SessionName,
                TermName = currentTerm?.Name,
                TermSequence = currentTerm?.SequenceNumber ?? 0,
            };

            return result;
        }
    }
}
