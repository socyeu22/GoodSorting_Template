using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LevelData")]

/// <summary>
/// ScriptableObject lưu trữ thông tin cấu hình cho từng màn chơi.
/// </summary>
public class LevelData : ScriptableObject
{
    [Tooltip("ID duy nhất của màn chơi.")]
    public int ID;

    [Tooltip("ID của scene(hình ảnh kệ sẽ được sử dụng) tương ứng với màn này.")]
    public int sceneID;

    [Tooltip("Thời gian giới hạn (giây) để hoàn thành màn.")]
    public int time;

    [Tooltip("Tổng số sản phẩm cần tạo trong suốt màn.")]
    public int totalProductCount;

    [Tooltip("Bộ sản phẩm được sử dụng để tạo màn chơi.")]
    // Bộ sản phẩm được sử dụng để tạo màn chơi, nếu là 10000 thì random, nếu khác 10000 thì sẽ sử dụng theo ID của bộ sản phẩm.
    public int dynamicProductSet;
    public int singlePush;
    public int roundsEmptyPlaceCount;
}
