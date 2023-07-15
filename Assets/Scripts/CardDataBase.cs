using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    void Awake()
    {
        cardList.Add (new Card(0, "None", 0, 0, 0,"None", Resources.Load <Sprite>("0"), Card.Element.NoElement, 0, 0, 0, 0, false, 0,0));
        cardList.Add (new Card(1, "Lính cầm khiên", 1, 3, 1, "Phòng thủ: Kẻ địch buộc phải tấn công lính này trước", Resources.Load<Sprite>("1"), Card.Element.NoElement, 0, 0,0, 0, false, 0,0));
        cardList.Add (new Card(2, "Thầy lang", 1, 2, 1, "Hồi Sức: Hồi 2 máu cho quân lính bất kỳ", Resources.Load<Sprite>("2"), Card.Element.NoElement, 0, 0, 0, 2, false, 0,0));
        cardList.Add (new Card(3, "Thuốc cầm máu 0", 0, 0, 1, "Hồi 3 máu cho quân lính bất kỳ", Resources.Load<Sprite>("3"), Card.Element.NoElement, 0, 0, 0, 3, true, 0,0));
        cardList.Add (new Card(4, "Lính cảm tử", 3, 1, 1, "Chớp nhoáng: Tấn công xong liền bị tiêu diệt", Resources.Load<Sprite>("4"), Card.Element.NoElement, 0, 0, 0, 0, false, 0,0));
        cardList.Add (new Card(5, "Phi tiêu 0", 0, 0, 1,"Gây 2 dame cho hero. Đặc biệt: Gây 4 dame nếu trên bàn đấu có Lính cảm tử", Resources.Load <Sprite>("5"), Card.Element.NoElement, 0, 0, 0, 0, true, 2,0));
        cardList.Add (new Card(6, "Rút bài", 0, 0, 2, "Rút thêm 2 quân bài", Resources.Load<Sprite>("6"), Card.Element.NoElement, 2, 0, 0, 0, true, 0,0));
        cardList.Add (new Card(7, "Lính du kích", 3, 2, 2, "Gây 2 dame vào quân lính bất kỳ", Resources.Load<Sprite>("7"), Card.Element.NoElement, 0, 0, 0, 0, false, 2,0));
        cardList.Add (new Card(8, "Lính tiếp viện", 2, 3, 2,"Rút 1 lá bài trong bộ bài", Resources.Load <Sprite>("8"), Card.Element.NoElement, 1, 0, 0, 0, false, 0,0));
        cardList.Add (new Card(9, "Lính cổ vũ", 2, 2, 2,"Tất cả quân bài phép thuật tăng 1 dame khi bàn đấu có quân này", Resources.Load <Sprite>("9"), Card.Element.NoElement, 0, 0, 0, 0, false, 0,1));
        cardList.Add (new Card(10, "Thuốc cầm máu 1", 0, 0, 2,"Hồi 3 máu cho 1 quân bất kỳ ,sau đó hồi 2 máu cho hero", Resources.Load <Sprite>("10"), Card.Element.NoElement, 0, 0, 0, 3, true, 0,0));
        cardList.Add (new Card(11, "Nguyễn Lữ", 5, 3, 3, "Sĩ Khí: Tăng tất cả lính có trên sân 1 dame 1 máu khi quân bài này còn trên sân", Resources.Load<Sprite>("11"), Card.Element.Fire, 0, 0,0, 0, false, 0,1));
        cardList.Add (new Card(12, "Phạm Văn Tham", 4, 3, 3,"Cố Thủ: Sau khi quân bài chết gây 1 dame toàn bộ quân địch, tăng 1 máu cho phe ta", Resources.Load <Sprite>("12"), Card.Element.NoElement, 0, 0, 0, 0, false, 1,0));
        cardList.Add (new Card(13, "Lính Gia Định", 3, 4, 3,"Phòng thủ : Kẻ địch buộc phải tấn công lính này trước", Resources.Load <Sprite>("13"), Card.Element.NoElement, 0, 0, 0, 0, false, 0,0));
        cardList.Add (new Card(14, "Phi tiêu 1", 0, 0, 3,"Gây 3 dame cho hero, Gây 5 dame nếu trên bàn đấu có Phạm Văn Tham", Resources.Load <Sprite>("14"), Card.Element.NoElement, 0, 0, 0, 0, true, 3,0));
        cardList.Add (new Card(15, "Trận tập kích", 0, 0, 3, "Gây 2 dame cho toàn bộ quân địch", Resources.Load<Sprite>("15"), Card.Element.NoElement, 0, 0, 0, 0, true, 2,0));
        cardList.Add (new Card(16, "Nguyễn Nhạc", 6, 4, 4,"Sĩ Khí : Tăng tất cả lính có trên sân 2 dame 2 máu khi quân bài này còn trên sân", Resources.Load <Sprite>("16"), Card.Element.Water, 0, 0, 0, 2, false, 0,2));
        cardList.Add (new Card(17, "Ngô Thì Nhậm", 4, 5, 4, "", Resources.Load<Sprite>("17"), Card.Element.Fire, 0, 0, 0, 0, false, 0,0));
        cardList.Add (new Card(18, "Phòng tuyến Tam Điệp", 0, 0, 4, "Gây 2 dame cho quân địch, nếu trên sân có Ngô Thì Nhậm thì gây 3 dame", Resources.Load<Sprite>("18"), Card.Element.NoElement, 0, 0, 0, 0, true, 2,0));
        cardList.Add (new Card(19, "Lính cầm khiên 1", 4, 5, 4, "Phòng thủ: Kẻ địch buộc phải tấn công lính này trước", Resources.Load<Sprite>("19"), Card.Element.NoElement, 0, 0,0, 0, false, 0,0));
        cardList.Add (new Card(20, "Thuốc cầm máu 2", 0, 0, 4, "Hồi 3 máu cho toàn bộ quân lính bên mình", Resources.Load<Sprite>("20"), Card.Element.NoElement, 0, 0, 0, 3, true, 0,0));
        cardList.Add (new Card(21, "Nguyễn Huệ", 6, 6, 5, "Sĩ Khí: Tăng tất cả lính có trên sân 3 dame 3 máu khi quân bài này còn trên sân. Khi trên sân có 3 anh em nhà Nguyễn: gây 3 dame cho toàn bộ quân địch trên sân trong lượt đó", Resources.Load<Sprite>("21"), Card.Element.Metal, 0, 0, 0, 3, false, 3,3));
        cardList.Add (new Card(22, "Lính cổ vũ 1", 4, 5, 5,"Tất cả quân bài phép thuật tăng 2 dame khi bàn đấu có quân này", Resources.Load <Sprite>("22"), Card.Element.NoElement, 0, 0, 0, 0, false, 0,2));
        cardList.Add (new Card(23, "Nguyễn Quang Toản", 5, 5, 5, "", Resources.Load<Sprite>("23"), Card.Element.Metal, 0, 0, 0, 0, false, 0,2));
        cardList.Add (new Card(24, "Bảo vệ Tây Sơn", 0, 0, 5, "Tăng 2 máu cho toàn bộ quân ta, gây 2 dame cho quân địch", Resources.Load<Sprite>("24"), Card.Element.NoElement, 0, 0, 0, 2, true, 2,0));
        cardList.Add (new Card(25, "Đại chiến quân Thanh", 0, 0, 5, "Gây 2 dame cho toàn bộ quân địch, nếu bên quân địch có Tôn Sĩ Nghị gây thêm 1 dame", Resources.Load<Sprite>("25"), Card.Element.NoElement, 0, 0, 0, 0, true, 2,0));
        cardList.Add (new Card(26, "Chiếu cầu hiền", 0, 0, 2, "Gọi lại 1 lá bài đã sử dụng", Resources.Load<Sprite>("26"), Card.Element.NoElement, 0, 0, 1, 0, true, 0,0));
    }
}
