﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TimeRegistrationDemo.Data;
using TimeRegistrationDemo.Data.Entities;
using TimeRegistrationDemo.Repositories.Interfaces;

namespace TimeRegistrationDemo.Repositories.Implementations
{
    public class HolidayRequestRepository : IHolidayRequestRepository
    {
        private readonly TimeRegistrationDbContext TimeRegistrationDbContext;

        public HolidayRequestRepository(TimeRegistrationDbContext timeRegistrationDbContext)
        {
            TimeRegistrationDbContext = timeRegistrationDbContext;
        }

        public void Register(HolidayRequestEntity holidayRequest)
        {
            TimeRegistrationDbContext.HolidayRequests.Add(holidayRequest);
            TimeRegistrationDbContext.SaveChanges();
        }

        public bool ExistsByToAndFrom(DateTime from, DateTime to, long userId)
        {
            return TimeRegistrationDbContext.HolidayRequests
                .Any(x => x.From == from && x.To == to && x.User.Id == userId);
        }

        public IList<HolidayRequestEntity> GetByYearAndUserId(int year, long userId)
        {
            return TimeRegistrationDbContext.HolidayRequests
                .Include(x => x.HolidayType)
                .Where(x => x.From.Date.Year == year && x.User.Id == userId)
                .ToList();
        }
    }
}
