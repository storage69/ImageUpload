Demo REST API service ImageUpload (ASP.NET MVC 5)

Details:
	- work with POST request only
	- uploading file content:
		1. from multipart/form-data
		2. from file URL params 
		3. from AJAX+JSON request with base64 encoded data string
	- can upload several images in all cases
	- move uploaded files to "uploaded_images" folder in root and make thumbnail 100x100
	- convert images to png format in all cases
	- Unit tests in additional project
	
Used:
	- Solution can be opened in Visual Studio 2013 and above (in VS 2019 also works well)

Running and Testing:
	- Open "buld" folder and click "run-docker.cmd" (or run "docker-compose up --build" in cmd) - will be used official docker image of Microsoft Server with IIS (~2.5 GB)
	- Open address in browser http://localhost:8000/ - will be opened testing web page