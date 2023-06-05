// using System;
// using UnityEngine;
//
// namespace ACTSkillDemo
// {
//     public class GameManager : MonoBehaviour
//     {
//         private static GameManager instance;
//
//         public static GameManager Instance
//         {
//             get
//             {
//                 if (instance) return instance;
//                 instance = FindObjectOfType<GameManager>();
//                 if (instance) return instance;
//                 var obj = new GameObject(nameof(GameManager));
//                 instance = obj.AddComponent<GameManager>();
//                 return instance;
//             }
//         }
//
//         private void Awake()
//         {
//             if (!instance)
//                 instance = this;
//             else if (instance != this)
//             {
//                 Destroy(this);
//                 return;
//             }
//             DontDestroyOnLoad(transform.root.gameObject);
//         }
//     }
// }
