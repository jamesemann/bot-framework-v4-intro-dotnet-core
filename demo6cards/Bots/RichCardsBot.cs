// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Bot;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Core.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Prompts.Choices;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;
using Newtonsoft.Json;

namespace demo6cards
{
    public class RichCardsBot : IBot
    {
        public RichCardsBot()
        {
        }

        public async Task OnTurn(ITurnContext context)
        {
            if (context.Activity.Type == ActivityTypes.Message)
            {

                var activity = context.Activity;
                switch (activity.Text)
                {
                    case "Adaptive card":
                        await context.SendActivity(CreateResponse(activity, CreateAdaptiveCardAttachment()));
                        break;
                    case "Animation card":
                        await context.SendActivity(CreateResponse(activity, CreateAnimationCardAttachment()));
                        break;
                    case "Audio card":
                        await context.SendActivity(CreateResponse(activity, CreateAudioCardAttachment()));
                        break;
                    case "Hero card":
                        await context.SendActivity(CreateResponse(activity, CreateHeroCardAttachment()));
                        break;
                    case "Receipt card":
                        await context.SendActivity(CreateResponse(activity, CreateReceiptCardAttachment()));
                        break;
                    case "Signin card":
                        await context.SendActivity(CreateResponse(activity, CreateSignInCardAttachment()));
                        break;
                    case "Thumbnail card":
                        await context.SendActivity(CreateResponse(activity, CreateThumbnailCardAttachment()));
                        break;
                    case "Video card":
                        await context.SendActivity(CreateResponse(activity, CreateVideoCardAttacment()));
                        break;
                    default: 
                        activity.Text = "";
                        activity.Text += $"Adaptive card{Environment.NewLine}";
                        activity.Text += $"Animation card{Environment.NewLine}";
                        activity.Text += $"Audio card{Environment.NewLine}";
                        activity.Text += $"Hero card{Environment.NewLine}";
                        activity.Text += $"Receipt card{Environment.NewLine}";
                        activity.Text += $"Signin card{Environment.NewLine}";
                        activity.Text += $"Thumbnail card{Environment.NewLine}";
                        activity.Text += $"Video card{Environment.NewLine}";
                        await context.SendActivity(activity);

                        break;
                }

            }
        }

        private Activity CreateResponse(Activity activity, Attachment attachment)
        {
            var response = activity.CreateReply();
            response.Attachments = new List<Attachment>() { attachment };
            return response;
        }

        private Attachment CreateAdaptiveCardAttachment()
        {
            var adaptiveCard = File.ReadAllText(@".\AdaptiveCard.json");
            return new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCard)
            };
        }

        private Attachment CreateAnimationCardAttachment()
        {
            return new AnimationCard()
            {
                Title = "Microsoft Bot Framework",
                Media = new List<MediaUrl>()
                    {
                        new MediaUrl("http://i.giphy.com/Ki55RUbOV5njy.gif")
                    },
                Subtitle = "Animation Card"
            }.ToAttachment();
        }

        private Attachment CreateAudioCardAttachment()
        {
            return new AudioCard()
            {
                Title = "I am your father",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl("http://www.wavlist.com/movies/004/father.wav")
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "Read more",
                        Value = "https://en.wikipedia.org/wiki/The_Empire_Strikes_Back"
                    }
                },
                Subtitle = "Star Wars: Episode V - The Empire Strikes Back",
                Text = "The Empire Strikes Back (also known as Star Wars: Episode V – The Empire Strikes Back) is a 1980 American epic space opera film directed by Irvin Kershner. Leigh Brackett and Lawrence Kasdan wrote the screenplay, with George Lucas writing the film\'s story and serving as executive producer. The second installment in the original Star Wars trilogy, it was produced by Gary Kurtz for Lucasfilm Ltd. and stars Mark Hamill, Harrison Ford, Carrie Fisher, Billy Dee Williams, Anthony Daniels, David Prowse, Kenny Baker, Peter Mayhew and Frank Oz.",
                Image = new ThumbnailUrl("https://upload.wikimedia.org/wikipedia/en/3/3c/SW_-_Empire_Strikes_Back.jpg")
            }.ToAttachment();
        }

        private Attachment CreateHeroCardAttachment()
        {
            return new HeroCard()
            {
                Title = "",
                Images = new List<CardImage>()
                {
                    new CardImage("https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg")
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "Get Started",
                        Value = "https://docs.microsoft.com/en-us/azure/bot-service/"
                    }
                }
            }.ToAttachment();
        }

        private Attachment CreateReceiptCardAttachment()
        {
            return new ReceiptCard()
            {
                Title = "John Doe",
                Facts = new List<Fact>()
                {
                    new Fact(key: "Order Number", value: "1234"),
                    new Fact(key: "Payment Method", value: "VISA 5555-****")
                },
                Items = new List<ReceiptItem>()
                {
                    new ReceiptItem()
                    {
                        Title = "Data Transfer",
                        Price = "$38.45",
                        Quantity = "368",
                        Image = new CardImage("https://github.com/amido/azure-vector-icons/raw/master/renders/traffic-manager.png")
                    },
                    new ReceiptItem()
                    {
                        Title = "App Service",
                        Price = "$45.00",
                        Quantity = "720",
                        Image = new CardImage("https://github.com/amido/azure-vector-icons/raw/master/renders/cloud-service.png")
                    }
                },
                Tax = "$7.50",
                Total = "$90.95",
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "More Information",
                        Value = "https://azure.microsoft.com/en-us/pricing/details/bot-service/"
                    }
                }
            }.ToAttachment();
        }

        private Attachment CreateSignInCardAttachment()
        {
            return new SigninCard()
            {
                Text = "BotFramework Sign-in Card",
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.Signin,
                        Title = "Sign-in",
                        Value = "https://login.microsoftonline.com"
                    }
                }
            }.ToAttachment();
        }

        private Attachment CreateThumbnailCardAttachment()
        {
            return new ThumbnailCard()
            {
                Title = "BotFramework Thumbnail Card",
                Images = new List<CardImage>()
                {
                    new CardImage()
                    {
                        Url = "https://sec.ch9.ms/ch9/7ff5/e07cfef0-aa3b-40bb-9baa-7c9ef8ff7ff5/buildreactionbotframework_960.jpg"
                    }
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "Get Started",
                        Value = "https://docs.microsoft.com/en-us/azure/bot-service/"
                    }
                },
                Subtitle = "Your bots — wherever your users are talking",
                Text = "Build and connect intelligent bots to interact with your users naturally wherever they are, from text/sms to Skype, Slack, Office 365 mail and other popular services."
            }.ToAttachment();
        }

        private Attachment CreateVideoCardAttacment()
        {
            return new VideoCard()
            {
                Title = "Big Buck Bunny",
                Media = new List<MediaUrl>()
                {
                    new MediaUrl("http://download.blender.org/peach/bigbuckbunny_movies/BigBuckBunny_320x180.mp4")
                },
                Buttons = new List<CardAction>()
                {
                    new CardAction()
                    {
                        Type = ActionTypes.OpenUrl,
                        Title = "Learn More",
                        Value = "https://peach.blender.org/"
                    }
                },
                Subtitle = "by the Blender Institute",
                Text = "Big Buck Bunny (code-named Peach) is a short computer-animated comedy film by the Blender Institute, part of the Blender Foundation. Like the foundation\'s previous film Elephants Dream, the film was made using Blender, a free software application for animation made by the same foundation. It was released as an open-source film under Creative Commons License Attribution 3.0."
            }.ToAttachment();
        }
    }
}