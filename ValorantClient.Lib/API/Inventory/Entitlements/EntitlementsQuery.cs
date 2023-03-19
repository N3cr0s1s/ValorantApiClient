using MediatR;

namespace ValorantClient.Lib.API.Inventory.Entitlements
{
    public class EntitlementsQuery : IRequest<EntitlementsResponse>
    {

        public string Type { get; set; }

        public class ItemType
        {
            const string SkinLevel = "e7c63390-eda7-46e0-bb7a-a6abdacd2433";
            const string SkinChroma = "3ad1b2b2-acdb-4524-852f-954a76ddae0a";
            const string Agent = "01bb38e1-da47-4e6a-9b3d-945fe4655707";
            const string ContractDefinition = "f85cb6f7-33e5-4dc8-b609-ec7212301948";
            const string Buddy = "dd3bf334-87f3-40bd-b043-682a57a8dc3a";
            const string Spray = "d5f120f8-ff8c-4aac-92ea-f2b5acbe9475";
            const string PlayerCard = "3f296c07-64c3-494c-923b-fe692a4fa1bd";
            const string PlayerTitle = "de7caa6b-adf7-4588-bbd1-143831e786c6";
        }
    }
}
