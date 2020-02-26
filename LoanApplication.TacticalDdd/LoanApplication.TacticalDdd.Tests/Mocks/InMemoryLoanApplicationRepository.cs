using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.Tests.Mocks
{
    public class InMemoryLoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly ConcurrentDictionary<LoanApplicationId, DomainModel.LoanApplication> applications = 
            new ConcurrentDictionary<LoanApplicationId, DomainModel.LoanApplication>();

        public InMemoryLoanApplicationRepository(IEnumerable<DomainModel.LoanApplication> initialData)
        {
            foreach (var application in initialData)
            {
                applications[application.Id] = application;
            }
        }
        public void Add(DomainModel.LoanApplication loanApplication)
        {
            applications[loanApplication.Id] = loanApplication;
        }

        public DomainModel.LoanApplication WithNumber(LoanApplicationNumber loanApplicationNumber)
        {
            return applications.Values.FirstOrDefault(a => a.Number == loanApplicationNumber);
        }
    }
}