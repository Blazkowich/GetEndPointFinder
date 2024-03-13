using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndPointFinder;
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class ActionPerformer
{
    public string userId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
}

public class Data
{
    public List<Order> Orders { get; set; }
    public int totalCount { get; set; }
}

public class Discount
{
    public int offerType { get; set; }
    public double price { get; set; }
    public string discountId { get; set; }
    public DiscountName discountName { get; set; }
    public DateTime dateTo { get; set; }
    public DiscountSubName discountSubName { get; set; }
    public bool? isSpecial { get; set; }
    public int? discountPriceInPoints { get; set; }
    public int? specialProductCnt { get; set; }
    public int? purchasedSpecialProductCnt { get; set; }
    public bool? isClub { get; set; }
    public int? maxLimitOnCustomer { get; set; }
    public int? discountCntOnMaxLimit { get; set; }
    public int? purchasedDiscountCntOnMaxLimit { get; set; }
}

public class DiscountName
{
    public string ge { get; set; }
    public string en { get; set; }
    public string ru { get; set; }
}

public class DiscountSubName
{
    public string ge { get; set; }
    public string en { get; set; }
}

public class ErpSyncInfo
{
    public DateTime erpSyncTime { get; set; }
    public string erpError { get; set; }
    public string erpSyncType { get; set; }
    public string erpResult { get; set; }
    public string erpOrderID { get; set; }
    public int erpSyncStatus { get; set; }
    public string erpDocNumber { get; set; }
}

public class Meta
{
    public DateTime alertNoficicationSendTime { get; set; }
    public bool? isDraftOverridden { get; set; }
}

public class Order
{
    public string _id { get; set; }
    public int status { get; set; }
    public string cartId { get; set; }
    public string warehouseId { get; set; }
    public string warehouseName { get; set; }
    public User user { get; set; }
    public string orderPhoneNumber { get; set; }
    public string recordType { get; set; }
    public List<Product> products { get; set; }
    public string orderNumber { get; set; }
    public double totalAmount { get; set; }
    public double shouldBePayed { get; set; }
    public double originalAmount { get; set; }
    public double originalProductAmount { get; set; }
    public string deliveryType { get; set; }
    public double deliveryPrice { get; set; }
    public int deliveryMinutes { get; set; }
    public string paymentType { get; set; }
    public string revertType { get; set; }
    public int erpSendCount { get; set; }
    public string selectedAddress { get; set; }
    public SelectedAddressCoordinates selectedAddressCoordinates { get; set; }
    public string addressComment { get; set; }
    public string userAddressesId { get; set; }
    public string orderComment { get; set; }
    public bool preventAutoAssignment { get; set; }
    public bool isPaymentCommitted { get; set; }
    public bool isProcessedByJob { get; set; }
    public string bankCardId { get; set; }
    public string zoneId { get; set; }
    public bool pickupIsConfirmed { get; set; }
    public double discountedAmount { get; set; }
    public double discountAmount { get; set; }
    public string sessionClient { get; set; }
    public bool isCheckedOnPaymentByJob { get; set; }
    public Meta meta { get; set; }
    public string customerCardNo { get; set; }
    public double point { get; set; }
    public int chargedPoints { get; set; }
    public int originalChargedPoints { get; set; }
    public int originalPoint { get; set; }
    public int surcharge { get; set; }
    public string paymentModel { get; set; }
    public bool orderErpSyncSuccess { get; set; }
    public ProcessingInformation processingInformation { get; set; }
    public int totalWeightOfProducts { get; set; }
    public int originalTotalWeightOfProducts { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public int __v { get; set; }
    public string paymentId { get; set; }
    public double originalAmountSpentByBankCard { get; set; }
    public int originalAmountSpentByWallet { get; set; }
    public DateTime paymentDate { get; set; }
    public bool autoAssignment { get; set; }
    public ActionPerformer actionPerformer { get; set; }
    public object finishedDate { get; set; }
    public object pickUpFinishTime { get; set; }
    public List<ErpSyncInfo> erpSyncInfo { get; set; }
    public string erpDocNumber { get; set; }
    public string erpOrderID { get; set; }
    public DateTime? erpSyncTime { get; set; }
    public string erpError { get; set; }
    public string erpResult { get; set; }
    public int? erpSyncStatus { get; set; }
}

public class ProcessingInformation
{
    public DateTime taskAssignmentStartTime { get; set; }
    public int taskAssignmentProcessCnt { get; set; }
    public double skuCollectionTime { get; set; }
    public double customerCallTime { get; set; }
    public int storeQueueWaitingTime { get; set; }
    public double totalCollectionTime { get; set; }
    public DateTime pickingPossibleFinishTime { get; set; }
    public bool isProcessedByJob { get; set; }
    public int pickerCollectionTime { get; set; }
    public DateTime? deliveryTime { get; set; }
}

public class Product
{
    public string prodId { get; set; }
    public string title { get; set; }
    public ProductTitles productTitles { get; set; }
    public string barCode { get; set; }
    public double quantity { get; set; }
    public double price { get; set; }
    public double originalPrice { get; set; }
    public double scannedQuantity { get; set; }
    public double paymentAmount { get; set; }
    public string imageId { get; set; }
    public int status { get; set; }
    public int weight { get; set; }
    public int totalWeight { get; set; }
    public string categoryId { get; set; }
    public Discount discount { get; set; }
}

public class ProductTitles
{
    public string ge { get; set; }
    public string en { get; set; }
    public string ru { get; set; }
}

public class Root
{
    public string status { get; set; }
    public Data Data { get; set; }
}

public class SelectedAddressCoordinates
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
}

public class User
{
    public string userId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string phoneNumber { get; set; }
    public string email { get; set; }
}