﻿using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(string id);
        Employee Create(Employee employee);
        Employee Update(Employee newEmployeeInfo);
    }
}
