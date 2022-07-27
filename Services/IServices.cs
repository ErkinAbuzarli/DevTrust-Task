using System;
using System.Collections.Generic;
using DevTrust_Task.DTOs;

namespace DevTrust_Task.Services
{
    public interface IServices
    {
        Object Deserialize(Object @object, string json);
    }
}
