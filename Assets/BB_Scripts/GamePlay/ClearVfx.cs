using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Điều khiển hiệu ứng khi ô được xóa khỏi lưới.
/// Chọn sprite ngẫu nhiên và thực hiện chuỗi animation phóng to, xoay
/// rồi làm mờ trước khi trả về trạng thái ban đầu.
/// </summary>
public class ClearVfx : MonoBehaviour
{
    /// <summary>SpriteRenderer của đối tượng hiệu ứng.</summary>
    private SpriteRenderer vfxSprite;

    /// <summary>Danh sách sprite sử dụng để tạo hiệu ứng ngẫu nhiên.</summary>
    public Sprite[] spriteArr;

    /// <summary>
    /// Khởi tạo: lấy SpriteRenderer và đưa hiệu ứng về trạng thái mặc định.
    /// </summary>
    void Start()
    {
        vfxSprite = GetComponent<SpriteRenderer>();
        ResetPara();
    }

    // Hàm Update để test nhanh hiệu ứng trong quá trình phát triển (đang tắt).
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //    PlayAnim();
    }

    /// <summary>
    /// Thực thi chuỗi animation khi ô bị xóa:
    /// 1. Cố định góc và hiện sprite.
    /// 2. Phóng to nhanh rồi xoay nhẹ để tạo cảm giác bùng nổ.
    /// 3. Tiếp tục phóng to và làm mờ dần trước khi reset.
    /// </summary>
    public void PlayAnim()
    {
        // Đặt góc ban đầu và màu về trạng thái hiển thị
        transform.rotation = Quaternion.Euler(0, 0, 5);
        vfxSprite.color = new Color(1, 1, 1, 1);

        // Giai đoạn 1: phóng to nhẹ
        transform.DOScale(new Vector3(-1.5f, 1.5f, 1.5f), 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, -20.0f), 0.35f).SetDelay(0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {


            });
            // Giai đoạn 2: xoay và phóng to thêm
            transform.DORotate(new Vector3(0, 0, -20.0f), 0.35f)
                .SetDelay(0.25f)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    // callback rỗng để có thể thêm hiệu ứng sau này
                });
            transform.DOScale(new Vector3(-2.5f, 2.5f, 2.5f), 0.35f).SetDelay(0.25f).SetEase(Ease.OutQuart);

            // Giai đoạn 3: mờ dần rồi reset về trạng thái chờ
            vfxSprite.DOFade(0.0f, 0.35f).SetDelay(0.25f).SetEase(Ease.Linear).OnComplete(() =>
            {
                ResetPara();
            });
        });
    }

    /// <summary>
    /// Đưa hiệu ứng về trạng thái chuẩn bị cho lần kích hoạt tiếp theo.
    /// </summary>
    private void ResetPara()
    {
        // Kích thước và màu mặc định ở trạng thái ẩn
        transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        vfxSprite.color = new Color(1, 1, 1, 0);

        // Chọn ngẫu nhiên một sprite để tạo cảm giác đa dạng
        int randomIndex = Random.Range(0, spriteArr.Length);
        vfxSprite.sprite = spriteArr[randomIndex];
    }
}
