using StereoKit;
using System;
using System.Threading;
using SeeShark;
using SeeShark.FFmpeg;
using SeeShark.Device;


// Create a callback for decoded camera frames
    void frameEventHandler(object? _sender, FrameEventArgs e)
    {
                Frame frame = e.Frame;

        // Get information and raw data from a frame
        Console.WriteLine($"New frame ({frame.Width}x{frame.Height} | {frame.PixelFormat})");
        Console.WriteLine($"Length of raw data: {frame.RawData.Length} bytes");
    }



// Initialize StereoKit
SKSettings settings = new SKSettings
{
	appName      = "XR-dissociation",
	assetsFolder = "Assets",
};
if (!SK.Initialize(settings))
	return;


// Create assets used by the app
Pose  cubePose = new Pose(0, 0, -0.5f);
Model cube     = Model.FromMesh(
	Mesh.GenerateRoundedCube(Vec3.One*0.1f, 0.02f),
	Material.UI);

Matrix   floorTransform = Matrix.TS(0, -1.5f, 0, new Vec3(30, 0.1f, 30));
Material floorMaterial  = new Material("floor.hlsl");
floorMaterial.Transparency = Transparency.Blend;


// Create a CameraManager to manage camera devices
using var manager = new CameraManager();

// Get the first camera available
using var camera = manager.GetDevice(0);

// Attach your callback to the camera's frame event handler
camera.OnFrame += frameEventHandler;

// Start decoding frames asynchronously
camera.StartCapture();



Tex sky = Tex.FromCubemapEquirectangular("sky.hdr");



// Core application loop
SK.Run(() =>
{
	if (Device.DisplayBlend == DisplayBlend.Opaque)
		Mesh.Cube.Draw(floorMaterial, floorTransform);

	UI.Handle("Cube", ref cubePose, cube.Bounds);
	cube.Draw(cubePose.ToMatrix());
	

	

	
	//sky = Tex.FromColors();
	Renderer.SkyTex = sky;
	
	
           
	
});
