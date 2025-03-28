// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Expensi.UIClient.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class ExpenseDto : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The amount property</summary>
        public decimal? Amount { get; set; }
        /// <summary>The categoryId property</summary>
        public Guid? CategoryId { get; set; }
        /// <summary>The categoryName property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CategoryName { get; set; }
#nullable restore
#else
        public string CategoryName { get; set; }
#endif
        /// <summary>The currency property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Currency { get; set; }
#nullable restore
#else
        public string Currency { get; set; }
#endif
        /// <summary>The date property</summary>
        public DateTimeOffset? Date { get; set; }
        /// <summary>The description property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Description { get; set; }
#nullable restore
#else
        public string Description { get; set; }
#endif
        /// <summary>The id property</summary>
        public Guid? Id { get; set; }
        /// <summary>The remitterId property</summary>
        public Guid? RemitterId { get; set; }
        /// <summary>The remitterName property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? RemitterName { get; set; }
#nullable restore
#else
        public string RemitterName { get; set; }
#endif
        /// <summary>The title property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>
        /// Instantiates a new <see cref="global::Expensi.UIClient.Models.ExpenseDto"/> and sets the default values.
        /// </summary>
        public ExpenseDto()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Expensi.UIClient.Models.ExpenseDto"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Expensi.UIClient.Models.ExpenseDto CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Expensi.UIClient.Models.ExpenseDto();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "amount", n => { Amount = n.GetDecimalValue(); } },
                { "categoryId", n => { CategoryId = n.GetGuidValue(); } },
                { "categoryName", n => { CategoryName = n.GetStringValue(); } },
                { "currency", n => { Currency = n.GetStringValue(); } },
                { "date", n => { Date = n.GetDateTimeOffsetValue(); } },
                { "description", n => { Description = n.GetStringValue(); } },
                { "id", n => { Id = n.GetGuidValue(); } },
                { "remitterId", n => { RemitterId = n.GetGuidValue(); } },
                { "remitterName", n => { RemitterName = n.GetStringValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteDecimalValue("amount", Amount);
            writer.WriteGuidValue("categoryId", CategoryId);
            writer.WriteStringValue("categoryName", CategoryName);
            writer.WriteStringValue("currency", Currency);
            writer.WriteDateTimeOffsetValue("date", Date);
            writer.WriteStringValue("description", Description);
            writer.WriteGuidValue("id", Id);
            writer.WriteGuidValue("remitterId", RemitterId);
            writer.WriteStringValue("remitterName", RemitterName);
            writer.WriteStringValue("title", Title);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
