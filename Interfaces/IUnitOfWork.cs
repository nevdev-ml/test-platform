﻿using Contracts;
using Core;
using System;

namespace Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Activity> Activity { get; }

        IRepository<Answer> Answer { get; }

        IRepository<Course> Course { get; }

        IRepository<Curator> Curator { get; }

        IRepository<Grade> Grade { get; }

        IRepository<Group> Group { get; }

        IRepository<Question> Question { get; }

        IRepository<Report> Report { get; }

        IRepository<Role> Role { get; }

        IRepository<Section> Section { get; }

        IRepository<Template> Template { get; }

        IRepository<Test> Test { get; }

        IRepository<Core.Type> Type { get; }

        IRepository<User> User { get; }

        IMMRepository<GroupSection> GroupSection { get; }

        IMMRepository<ReportQA> ReportQA { get; }

        IMMRepository<UserTask> UserTask { get; }

        IRepository<Task> Task { get; }

        void Save();
    }
}