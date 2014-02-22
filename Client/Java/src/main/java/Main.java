import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import org.apache.http.HttpEntity;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.entity.ContentType;
import org.apache.http.entity.mime.MultipartEntityBuilder;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClients;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;

public class Main {
    public final static String SAMPLE_IMAGE_PATH = "../SampleImages";

    public static void main (String args[]) throws IOException {

        ImageSet imageSet = new ImageSet();
        imageSet.name = "Image Set";
        imageSet.images = new ArrayList<Image>();

        File dir = new File(SAMPLE_IMAGE_PATH);
        for(File file : dir.listFiles()){

            if(!file.getName().endsWith(".jpg") && !file.getName().endsWith(".png"))
                continue;

            Path path = Paths.get(file.getPath());

            Image image = new Image();
            image.fileName = file.getName();
            image.mimeType = Files.probeContentType(path);
            image.imageData = Files.readAllBytes(path);

            imageSet.images.add(image);
        }

        sendImageSet(imageSet);
    }

    private static void sendImageSet(ImageSet imageSet) throws IOException {

        MultipartEntityBuilder builder = MultipartEntityBuilder.create();

        Gson gson = new GsonBuilder()
                .excludeFieldsWithoutExposeAnnotation()
                .create();

        String imageSetJson = gson.toJson(imageSet);
        builder.addTextBody("imageset", imageSetJson, ContentType.APPLICATION_JSON);

        for(Image image : imageSet.images) {
            builder.addBinaryBody("images", image.imageData, ContentType.create(image.mimeType), image.fileName);
        }

        HttpEntity multipartHttpEntity = builder.build();

        HttpPost httpPost = new HttpPost("http://localhost:53908/api/send");
        httpPost.setEntity(multipartHttpEntity);

        CloseableHttpClient httpclient = HttpClients.createDefault();

        try {
            CloseableHttpResponse response = httpclient.execute(httpPost);
            response.getEntity().writeTo(System.out);
            response.close();
        }
        finally {
            httpclient.close();
        }
    }
}
