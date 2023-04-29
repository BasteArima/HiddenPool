using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

public class IAPSystem : MonoBehaviour, IStoreListener
{
    [SerializeField] private List<IAPButton> _nonConsumableButtons = new List<IAPButton>();

    private AppData _data;
    private IStoreController _controller;
    private IExtensionProvider _extensions;

    [Inject]
    private void Construct(AppData data)
    {
        _data = data;
    }

    private void Awake()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("remove_ads", ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _controller = controller;
        _extensions = extensions;
        RemovePurchasedNonConsumableProducts();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"Initialize IAP Failed with error: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"Initialize IAP Failed with error: {error}");
        Debug.Log($"Initialize IAP Failed with error message: {message}");
    }

    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    {
        return PurchaseProcessingResult.Complete;
    }
    
    private void RemovePurchasedNonConsumableProducts()
    {
        foreach (var product in _controller.products.all)
        {
            if (null != product && product.hasReceipt)
            {
                var nonConsumableButton = _nonConsumableButtons.Find(x => x.productId == product.definition.id);
                if (nonConsumableButton != null)
                {
                    nonConsumableButton.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Debug.Log($"Failed to buy: {product.definition.id}. Error: {purchaseFailureReason}");
    }
    
    public void OnPurchaseRemoveReward(Product product)
    {
        RemovePurchasedNonConsumableProducts();
        Debug.Log("OnPurchaseRemoveReward");
        AdsManagerSystem.Instance.ShowAds = false;
    }
}
