﻿using david.hotelbooking.domain.Concretes;
using Microsoft.EntityFrameworkCore;

namespace david.hotelbooking.UnitTests.Services
{
    public class RemoteMySqlDbcontextFactory
    {

        public UserDbContext GetUserContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                        .UseMySql("server=bd8qkqg3jffjiwkgnlyc-mysql.services.clever-cloud.com; user=uriilvzrrptwkye2;password=qODCkgKQu7gWzEwGb4z6;database=bd8qkqg3jffjiwkgnlyc",
                        op => op.EnableRetryOnFailure())
                        .Options;
            var dbContext = new UserDbContext(options);

            return dbContext;

        }

    }
}