using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;

namespace contosoBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity) // This is a http post that is looking for data 
            // from the body 
        {
            if (activity.Type == ActivityTypes.Message) // here the activity is a message 
            {
                // WRITE CODE HERE 
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // The connector client is equating the variable connector to a new connector client with a uri that does the activity 
                // service 
                var stockSymbol = activity.Text;
                var replyMessage = await GetStock(stockSymbol);

                // return our reply to the user
                Activity reply = activity.CreateReply(replyMessage); // we need to send back the reply message Thats what
                // activity reply is doing here. 
                await connector.Conversations.ReplyToActivityAsync(reply); 
                // here we are connecting to the connecter variable we created earlier using the connector client 
                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        // creating a small function 
        private async Task<string> GetStock(string symbol)
        {
            var stockItem = await Sockservices.Stock.GetStockPrice(symbol);
            string returnValue;
            if("FAIL" == stockItem.Status)
            {
                returnValue = $"I'm sorry I could not find any stock with symbol '{symbol}'";
            }
            else
            {
                returnValue = $"Stock: {stockItem.Symbol}, Price: ${stockItem.LastPrice}"; 
            }
            return returnValue; 
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