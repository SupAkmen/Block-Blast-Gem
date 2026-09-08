using UnityEngine;
using UnityEngine.SceneManagement;

namespace PopUps
{
    public class PopUpWithCurrencyLabel : PopUp
    {
        private UIPanel _panel;

        public override void AfterShowAnimation()
        {
            base.AfterShowAnimation();
            
            _panel = FindObjectOfType<UIPanel>(true);
            _panel?.gameObject.SetActive(true);
        }

        public override void Close()
        {
            base.Close();

            if (SceneManager.GetActiveScene().name == "Main")
            {
                _panel?.gameObject.SetActive(false);
            }
        }

        protected void ShowCoinSpendFX(Vector3 position)
        {
            SoundBase.instance.PlaySound(SoundBase.instance.coinsSpend);
            var fx = Instantiate(Resources.Load<GameObject>("FX/CoinSpendFX"), position, Quaternion.identity, transform.parent);
            fx.transform.localScale = Vector3.one;
        }
    }
}