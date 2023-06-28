using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        cardList.Add (new Card(0, "None", 0, 0, 0,"None", Resources.Load <Sprite>("0"), "Black", 0, 0, 0, 0, false, 0));
        cardList.Add (new Card(1, "Lính cầm khiên", 1, 3, 1, "Phòng thủ: Kẻ địch buộc phải tấn công quái vật này trước", Resources.Load<Sprite>("0"), "Black", 0, 0,0, 0, false, 0));
        cardList.Add (new Card(2, "Y tá", 1, 2, 1, "Hồi Sức: Hồi 2 máu cho hero", Resources.Load<Sprite>("0"), "Black", 0, 0, 0, 2, false, 0));
        cardList.Add (new Card(3, "Lính du kích", 3, 2, 2, "Gây 2 dame vào quân lính bất kì", Resources.Load<Sprite>("0"), "Black", 0, 0, 0, 0, false, 0));
        cardList.Add (new Card(4, "Nguyễn Lữ", 5, 3, 3, "Sĩ Khí: Tăng tất cả lính có trên sân 1 dame 1 máu khi quân bài này còn trên sân", Resources.Load<Sprite>("0"), "Red", 0, 0,0, 0, false, 0));
        cardList.Add(new Card(5, "Rút bài", 1, 1, 1, "Rút thêm 1 quân bài", Resources.Load<Sprite>("0"), "Black", 1, 0, 0, 0, false, 0));
        cardList.Add(new Card(6, "Thêm mana", 1, 1, 1, "Tăng thêm 1 mana", Resources.Load<Sprite>("0"), "Black", 0, 1, 0, 0, false, 0));
        cardList.Add(new Card(7, "Pháp sư", 1, 1, 1, "Gọi hồn 1 Card", Resources.Load<Sprite>("0"), "Black", 0, 0, 1, 0, false, 0));
        cardList.Add(new Card(8, "Trận tập kích", 0, 0, 3, "Gây 2 dame cho toàn bộ quân địch", Resources.Load<Sprite>("0"), "Black", 0, 0, 0, 0, true, 2));
        cardList.Add(new Card(9, "Thuốc cầm máu 2", 0, 0, 4, "Hồi 3 máu cho toàn bộ quân lính bên mình", Resources.Load<Sprite>("0"), "Black", 0, 0, 0, 3, true, 0));
    }
}
