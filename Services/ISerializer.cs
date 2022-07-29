using System;
using System.Collections.Generic;
using DevTrust_Task.DTOs;

namespace DevTrust_Task.Services
{
    public interface ISerializer
    {
        Object Deserialize(dynamic obj, string json);
        string Serialize(dynamic obj);
    }
}
