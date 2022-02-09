using AssessmentSvc.Core.Utils;
using DinkToPdf.Contracts;
using Moq;
using Shared.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentSvc.Core.Test.Mocks
{
    public class MockToPDF
    {
        public Mock<IToPDF> Mock { get; }

        public MockToPDF()
        {
            Mock = new Mock<IToPDF>();

            Mock.Setup(m => 
                m.ResultToPDF(
                    It.IsAny<object>(),
                    It.IsAny<string>(),
                    It.IsAny<List<TableObject<object>>>(), 
                    It.IsAny<List<KeyValuePair<string, IEnumerable<TableObject<object>>>>>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>()
                    )
                ).Returns("success");
        }
    }
}
