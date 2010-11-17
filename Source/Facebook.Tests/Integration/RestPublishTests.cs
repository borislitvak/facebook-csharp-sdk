﻿// --------------------------------
// <copyright file="RestPublishTests.cs" company="Facebook C# SDK">
//     Microsoft Public License (Ms-PL)
// </copyright>
// <author>Nathan Totten (ntotten.com) and Jim Zimmerman (jimzimmerman.com)</author>
// <license>Released under the terms of the Microsoft Public License (Ms-PL)</license>
// <website>http://facebooksdk.codeplex.com</website>
// ---------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Facebook.Tests.Rest
{
    [TestClass]
    public class RestPublishTests
    {

        [TestMethod]
        [TestCategory("RequiresOAuth")]
        public void publish_photo_to_application_album()
        {

#if DEBUG
            string photoPath = @"..\..\..\Facebook.Tests\bin\Debug\monkey.jpg";
#else
            string photoPath = @"..\..\..\Facebook.Tests\bin\Release\monkey.jpg";
#endif

            byte[] photo = File.ReadAllBytes(photoPath);
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.access_token = ConfigurationManager.AppSettings["AccessToken"];
            parameters.caption = "This is a test photo of a monkey that has been uploaded " +
                                 "by the Facebook C# SDK (http://facebooksdk.codeplex.com)" +
                                 "using the REST API";
            parameters.method = "facebook.photos.upload";
            parameters.uid = ConfigurationManager.AppSettings["UserId"];
            var mediaObject = new FacebookMediaObject
            {
                FileName = "monkey.jpg",
                ContentType = "image/jpeg",
            };
            mediaObject.SetValue(photo);
            parameters.source = mediaObject;
            dynamic result = app.Api(parameters, HttpMethod.Post);

            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.aid, null);
        }


        [TestMethod]
        public void Publish_Global_News()
        {
            FacebookApp app = new FacebookApp();
            dynamic parameters = new ExpandoObject();
            parameters.method = "dashboard.addGlobalNews";

            var list = new List<object>();
            dynamic news1 = new ExpandoObject();
            news1.message = "This is a test news message. " + DateTime.UtcNow.Ticks.ToString();
            list.Add(news1);

            parameters.news = list;

            dynamic result = app.Api(parameters, HttpMethod.Post);

            long id;
            long.TryParse(result, out id);
            Assert.IsTrue(id > 0);
        }
    }
}