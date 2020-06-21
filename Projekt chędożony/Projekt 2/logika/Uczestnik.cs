using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_2.logika
{
    class Uczestnik
    {
        private int ID;
        string sciezka_cert;
        X509Certificate2 cert;
        string haslo_cert = "Qwerty";

        bool pierwszy = true;
        

        Krypto krypto;
        bool rsaUsing = true;

        Game game;

        public Uczestnik(int id, string sciezka_cert, Game game)
        {
            this.game = game;
            this.ID = id;
            this.sciezka_cert = sciezka_cert + "g"+ID.ToString()+"cert.p12";
            this.cert = new X509Certificate2(this.sciezka_cert, haslo_cert);
            krypto = new Krypto(cert);


         
        }

        public void Greatings()
        {
            string s = "HelloClient";
            //s = s + krypto.GetPublicKeyXML();
            Print(s);
            SEND(s, true);
        }

        private void SendAESData()
        {
            string s = "AESData|"+ BitConverter.ToString( krypto.AESKeyGet()) ;
            SEND(s, true);
            rsaUsing = false;
            Print("koniec powitania");
        }

        private void ActualGame()
        {
            /////////!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



        }


        private void SEND(string s, bool rsa)
        {
            byte[] wiadomosc;
            if (rsa)
            {
                wiadomosc = krypto.ZakodujRSA(s);
            }
            else
            {
                wiadomosc = krypto.ZakodujAES(s);
            }


            game.RECIVE(wiadomosc, ID);
        }

        public void RECIVE(byte[] s)
        {
            string wiado;
            if (pierwszy)
            {
                wiado = Encoding.UTF8.GetString(s);
                
            }
            else
            {

                if (rsaUsing)
                {
                    wiado = krypto.OdkodujRSA(s);
                }
                else
                {
                    wiado = krypto.OdkodujAES(s);
                }
            }
            ManageMessages(wiado);
        }

        private void ManageMessages(string s)
        {
            string[] temp = s.Split('|');
            switch (temp[0])
            {
                case "HelloServer":

                    krypto.Set_HisPubklicKey(temp[1]);
                    SendAESData();

                    break;
                
                case "OkAES":

                    break;
                    
            }

        }

        private void Print(String s)
        {
            Console.WriteLine("Uczestnik {0}: {1}",ID,s);
        }

    }
}
