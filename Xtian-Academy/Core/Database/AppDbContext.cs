using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.Database
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserVerification> UserVerifications { get; set; }
        public DbSet<ApplicantDocumment> ApplicantDocumments { get; set; }
        public DbSet<TrainingVideos> TrainingVideos { get; set; }
        public DbSet<TrainingCourse> TrainingCourse { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<ProjectTopic> ProjectTopics { get; set; }
        public DbSet<Paystack> Paystacks { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<TestResult> TestResults { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<ReoccuringPayments> ReoccuringPayments { get; set; }
        public DbSet<SalaryRetures> SalaryRetureHistory { get; set; }
        public DbSet<EmployementData> EmployementData { get; set; }
        public DbSet<ExamDuration> ExamDuration { get; set; }
        public DbSet<InterviewQuestions> InterviewQuestions { get; set; }
        public DbSet<InterviewAnswerOptions> InterviewAnswerOptions { get; set; }
        public DbSet<InterviewTestResult> InterviewTestResults { get; set; }
        public DbSet<AnswerOptions> AnswerOptions { get; set; }
        public DbSet<TestQuestions> TestQuestions { get; set; }


    }
}
