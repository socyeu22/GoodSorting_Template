using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject lưu trữ các cấu hình bộ sản phẩm cho từng màn chơi

[System.Serializable]
[CreateAssetMenu(fileName = "ProductSet")]
public class ProductSet : ScriptableObject
{
    // Danh sách các bộ sản phẩm được sử dụng trong game
    public List<ProductItem> productSetList = new List<ProductItem>();
}

[System.Serializable]

public class ProductItem
{
    // Mã định danh của bộ sản phẩm
    public int ID;

    // Danh sách các ID sản phẩm thuộc về bộ này
    public List<int> productList;
}