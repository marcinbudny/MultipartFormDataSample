import com.google.gson.annotations.Expose;

import java.util.List;

public class ImageSet {

    @Expose
    public String name;

    @Expose(serialize = false, deserialize = false)
    public List<Image> images;
}
