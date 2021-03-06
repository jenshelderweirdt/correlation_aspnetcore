﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Digipolis.Correlation;
using Digipolis.Correlation.UnitTests.Utilities;
using Xunit;

namespace Digipolis.Correlation.UnitTests.CorrelationId
{
    public class CorrelationContextTests
    {
        [Fact]
        private void ThrowExceptionWhenOptionsIsNull()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CorrelationContext(Options.Create<CorrelationOptions>(null)));
        }

        [Fact]
        private void SetHeaderKeysFromOptions()
        {
            var options = new CorrelationOptions();

            var context = new CorrelationContext(Options.Create(options));

            Assert.Equal(options.IdHeaderKey, context.IdHeaderKey);
            Assert.Equal(options.SourceHeaderKey, context.SourceHeaderKey);
        }

        [Fact]
        public void SetValuesFirstTime()
        {
            var options = new CorrelationOptions();
            var context = new CorrelationContext(Options.Create(options));
            var id = Guid.NewGuid().ToString();
            var sourceId = Guid.NewGuid().ToString();
            var sourceName = "appName";
            var instanceId = Guid.NewGuid().ToString();
            var instanceName = "appName-instanceName";
            var userId = "userId";
            var ipAddress = "194.25.76.122";

            var result = context.TrySetValues(id, sourceId, sourceName, instanceId, instanceName, userId, ipAddress);

            Assert.True(result);
            Assert.Equal(id, context.Id);
            Assert.Equal(sourceId, context.SourceId);
            Assert.Equal(sourceName, context.SourceName);
            Assert.Equal(instanceId, context.InstanceId);
            Assert.Equal(instanceName, context.InstanceName);
            Assert.Equal(userId, context.UserId);
            Assert.Equal(ipAddress, context.IpAddress);
        }

        [Fact]
        public void KeepFirstTimeValues()
        {
            var options = new CorrelationOptions();
            var context = new CorrelationContext(Options.Create(options));
            var id = Guid.NewGuid().ToString();
            var sourceId = Guid.NewGuid().ToString();
            var sourceName = "appName";
            var instanceId = Guid.NewGuid().ToString();
            var instanceName = "appName-instanceName";
            var userId = "userId";
            var ipAddress = "194.25.76.122";

            context.TrySetValues(id, sourceId, sourceName, instanceId, instanceName, userId, ipAddress);
            var result = context.TrySetValues(id, sourceId, "otherSoure", instanceId, instanceName, userId, ipAddress);

            Assert.False(result);
            Assert.Equal(id, context.Id);
            Assert.Equal(sourceId, context.SourceId);
            Assert.Equal(sourceName, context.SourceName);
            Assert.Equal(instanceId, context.InstanceId);
            Assert.Equal(instanceName, context.InstanceName);
            Assert.Equal(userId, context.UserId);
            Assert.Equal(ipAddress, context.IpAddress);
        }
    }
}
