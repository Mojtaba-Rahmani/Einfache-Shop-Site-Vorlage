using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.Generator
{
    public class NameGenerator
    {
        public static string GeneratUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string GeneratUniqUserId()
        {
            return DateTime.Now.Month.ToString().Replace("-", "") + DateTime.Now.Second.ToString().Replace("-", "");
        }

    }
}
