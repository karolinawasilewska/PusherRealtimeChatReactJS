using PusherRealtimeChat.WebAPI.Models;
using PusherServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace PusherRealtimeChat.WebAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    public class MessagesController : ApiController
    {
        private static List<ChatMessage> messages =
              new List<ChatMessage>()
              {
                new ChatMessage
                {
                    AuthorTwitterHandle = "Pusher",
                    Text = "Hi there! ?"
                },
                new ChatMessage
                {
                    AuthorTwitterHandle = "Pusher",
                    Text = "Welcome to your chat app"
                }
              };

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(
                HttpStatusCode.OK,
                messages);
        }

        public HttpResponseMessage Post(ChatMessage message)
        {
            if (message == null || !ModelState.IsValid)
            {
                return Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest,
                    "Invalid input");
            }
            messages.Add(message);

            var pusher = new Pusher(
                "612065",
                "956fd390206ded8ef0c7",
                "5f06b71584d86c62ad2e",
                   new PusherOptions
                   {
                       Cluster = "eu"
                   });
            pusher.TriggerAsync(
                channelName: "messages",
                eventName: "new_message",
                data: new
                {
                    AuthorTwitterHandle = message.AuthorTwitterHandle,
                    Text = message.Text
                });

     return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}