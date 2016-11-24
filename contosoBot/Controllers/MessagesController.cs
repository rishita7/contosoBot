using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using contosoBot.Services;
using Microsoft.Bot.Builder.Luis;
using contosoBot.Sockservices;
//using dbo.BuildVersion.sql; 

namespace contosoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {


        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        /// the bot is running and we arnt holding up the bot while we're waiting for the API responce
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity) // This is a http post that is looking for data 
                                                                                 // from the body 
                                                                                 // the message controller is passing the whole activity class
        {
            if (activity.Type == ActivityTypes.Message) // here the activity is a message 
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // The connector client is equating the variable connector to a new connector client with a uri that does the activity 
                // WRITE CODE HERE 

                StateClient stateClient = activity.GetStateClient();
                BotData userData = await stateClient.BotState.GetUserDataAsync(activity.ChannelId, activity.From.Id);

                contosoBot.Services.StockItem stock1;
                //var message = contosoBot.dbo.BuildVersion.sql; 

                var phrase = activity.Text;

                HttpClient client = new HttpClient();
                string rawData = await client.GetStringAsync(new Uri("http://dev.markitondemand.com/Api/v2/Quote/json?symbol=" + activity.Text));
                stock1 = JsonConvert.DeserializeObject<StockItem>(rawData);

                string name = stock1.Name;
                float ChangePercentYTD = stock1.ChangePercentYTD;
                string Time = stock1.Timestamp;
                float MarketCap = stock1.MarketCap;
                float Open = stock1.Open;
                string symbol = stock1.Symbol;
                float LastPrice = stock1.LastPrice;

                // adding the HERO CARD
                Activity stockReply = activity.CreateReply($"Stock for the {name}");
                stockReply.Recipient = activity.From;
                stockReply.Type = "message";
                stockReply.Attachments = new List<Attachment>();

                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: "http://megaicons.net/static/img/icons_title/31/98/title/ios7-stock-icon.png"));

                List<CardAction> cardButtons = new List<CardAction>();
                CardAction plButton = new CardAction()
                {
                    Value = "http://www.nasdaq.com/symbol/" + symbol + "/interactive-chart",
                    Type = "openUrl",
                    Title = "More Info"
                };
                cardButtons.Add(plButton);



                ThumbnailCard plCard = new ThumbnailCard()
                {
                    Title = name + ":",
                    Subtitle = " On " + Time + " Stock Price is: $" + LastPrice + "  With a market capitalization of: " + MarketCap + "  And its Change in Year to Date percentage is: " + ChangePercentYTD + "%",
                    Images = cardImages,
                    Buttons = cardButtons
                };

                Attachment plAttachment = plCard.ToAttachment();
                stockReply.Attachments.Add(plAttachment);
                await connector.Conversations.SendToConversationAsync(stockReply);
            }

            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
