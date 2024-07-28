using AutoMapper;
using JobManagement.Domain.AggregatesModel.JobAggregate;
using JobManagement.Domain.AggregatesModel.JobAggregate.Entities;

namespace JobManagement.Application.Queries
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Job, JobViewModel>();

            CreateMap<Profession, ProfessionViewModel>();

            CreateMap<Skill, SkillViewModel>();

            CreateMap<Proposal, ProposalViewModel>();

            CreateMap<Question, QuestionViewModel>();

            CreateMap<Answer, AnswerViewModel>();
        }

    }
}
