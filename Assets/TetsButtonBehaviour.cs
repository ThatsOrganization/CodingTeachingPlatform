using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

class TetsButtonBehaviour : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("Menu");
    }
}
