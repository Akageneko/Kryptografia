import java.io.*;
import java.security.InvalidKeyException;
import java.security.Key;
import java.security.KeyStore;
import java.security.NoSuchAlgorithmException;
import java.util.Arrays;

import javax.crypto.BadPaddingException;
import javax.crypto.Cipher;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;


public class EncDec {

    private String TRANSFORMATION = "AES";

    public EncDec(){

    }

    public void encrypt(String alg, InputStream keyStoreFile, String keyPassword, String keyAlias, File inputFile, File outputFile)
           {
        doCrypto(Cipher.ENCRYPT_MODE, alg, keyStoreFile, keyPassword, keyAlias, inputFile, outputFile);
    }

    public void decrypt(String alg, InputStream keyStoreFile, String keyPassword, String keyAlias, File inputFile, File outputFile)
            {
        doCrypto(Cipher.DECRYPT_MODE, alg, keyStoreFile, keyPassword, keyAlias, inputFile, outputFile);
    }

    private void doCrypto(int cipherMode, String alg, InputStream keyStoreFile, String keyPassword, String keyAlias, File inputFile, File outputFile)   {
        try {


            KeyStore keyStore = KeyStore.getInstance(KeyStore.getDefaultType());
            keyStore.load(keyStoreFile, keyPassword.toCharArray());
            //KeyStore.ProtectionParameter entryPassword = new KeyStore.PasswordProtection(keyPassword.toCharArray());
            Key keyEntry = keyStore.getKey(keyAlias, keyPassword.toCharArray());

            TRANSFORMATION += "/" + alg + "/PKCS5Padding";

            // Key secretKey = new SecretKeySpec(key.getBytes(), ALGORITHM);


            FileInputStream inputStream = new FileInputStream(inputFile);
            byte[] inputBytes = new byte[(int) inputFile.length()];
            inputStream.read(inputBytes);


            Cipher cipher = Cipher.getInstance(TRANSFORMATION);
            byte[] iv = new byte[16];
            if (cipherMode == Cipher.ENCRYPT_MODE) {
                cipher.init(cipherMode, keyEntry);
                iv = cipher.getIV();
            } else{
                iv = Arrays.copyOf(inputBytes, 16);
                inputBytes = Arrays.copyOfRange(inputBytes, 16, inputBytes.length);
                cipher.init(cipherMode, keyEntry, new IvParameterSpec(iv));
            }


            byte[] outputBytes = cipher.doFinal(inputBytes);

            FileOutputStream outputStream = new FileOutputStream(outputFile);
            if (cipherMode == Cipher.ENCRYPT_MODE) {
                outputStream.write(iv);
            }
            outputStream.write(outputBytes);

            inputStream.close();
            outputStream.close();

        } catch (Exception ex) {
            System.out.println("Error: "+ ex);
            System.out.println(ex.getStackTrace());
        }
    }



    public String encrypt(String alg, InputStream keyStoreFile, String keyPassword, String keyAlias, String in)
    {
        return doCrypto(Cipher.ENCRYPT_MODE, alg, keyStoreFile, keyPassword, keyAlias, in);
    }

    public String decrypt(String alg, InputStream keyStoreFile, String keyPassword, String keyAlias, String in)
    {
        return doCrypto(Cipher.DECRYPT_MODE, alg, keyStoreFile, keyPassword, keyAlias, in);
    }

    private String doCrypto(int cipherMode, String alg, InputStream keyStoreFile, String keyPassword, String keyAlias, String in)   {
        String out = "";
        try {


            KeyStore keyStore = KeyStore.getInstance(KeyStore.getDefaultType());
            keyStore.load(keyStoreFile, keyPassword.toCharArray());
            //KeyStore.ProtectionParameter entryPassword = new KeyStore.PasswordProtection(keyPassword.toCharArray());
            Key keyEntry = keyStore.getKey(keyAlias, keyPassword.toCharArray());

            TRANSFORMATION += "/" + alg + "/PKCS5Padding";

            // Key secretKey = new SecretKeySpec(key.getBytes(), ALGORITHM);

            byte[] inputBytes = new byte[ in.length()];
            inputBytes = in.getBytes();


            Cipher cipher = Cipher.getInstance(TRANSFORMATION);
            byte[] iv = new byte[16];
            if (cipherMode == Cipher.ENCRYPT_MODE) {
                cipher.init(cipherMode, keyEntry);
                iv = cipher.getIV();
            } else{
                iv = Arrays.copyOf(inputBytes, 16);
                inputBytes = Arrays.copyOfRange(inputBytes, 16, inputBytes.length);
                cipher.init(cipherMode, keyEntry, new IvParameterSpec(iv));
            }


            byte[] outputBytes = cipher.doFinal(inputBytes);

            out = "";
            if (cipherMode == Cipher.ENCRYPT_MODE) {
                out = new String(iv);
            }
            out += new String(outputBytes);


        } catch (Exception ex) {
            System.out.println("Error: "+ ex);
            System.out.println(ex.getStackTrace());
        }
        return out;
    }

}