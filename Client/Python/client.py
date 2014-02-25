import urllib2
import MultipartPostHandler
import json
import os
import cookielib

def send_image_set (image_set, images):
	
	image_set_json = json.dumps(image_set)
	multipart_data = dict({"imageset" : image_set_json}.items() + images.items())
	
	cookies = cookielib.CookieJar()
	opener = urllib2.build_opener(urllib2.HTTPCookieProcessor(cookies), MultipartPostHandler.MultipartPostHandler)
	response = opener.open("http://localhost:53908/api/send", multipart_data)
	print response.read()
	

image_set = {
				"name" : "Image Set"
			}
			
images = {}
counter = 0
for dirpath, dnames, fnames in os.walk("..\\SampleImages"):
    for file in fnames:
		if file.endswith(".jpg") or file.endswith(".png"):
			images["image%d" % counter] = open(os.path.join(dirpath, file), "rb")
			counter += 1

send_image_set(image_set, images)

