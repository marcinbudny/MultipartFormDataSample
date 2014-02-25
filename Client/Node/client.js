var FormData = require('form-data');
var fs = require('fs');
var mime = require('mime');

function sendImageSet(imageSet, images) {
	var form = new FormData();
	
	form.append('imageset', JSON.stringify(imageSet), {
		contentType: 'application/json',
		// the library will only allow to specify content type 
		// if filename is also specified
		filename: 'dummy'
	});
	
	for(index in images) {
		form.append('image' + index, images[index].imageData, {
			contentType: images[index].mimeType,
			filename: images[index].fileName
		});
	}
	
	form.submit('http://localhost:53908/api/send', function(err, res) {
		res.on('data', function(data) {
			process.stdout.write(data); 
		});
		res.resume();
	});
}

var path = '../SampleImages';
var files = fs.readdirSync(path);

var imageSet = { name: 'Image Set' };
var images = [];

for(var index in files) {
	images.push({
		fileName: files[index],
		mimeType: mime.lookup(files[index]),
		imageData: fs.readFileSync('../SampleImages/' + files[index])
	});
}

// console.log(files);
// console.log(images);

sendImageSet(imageSet, images);
