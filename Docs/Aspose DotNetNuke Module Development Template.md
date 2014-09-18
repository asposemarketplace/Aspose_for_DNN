<div>
<h2>Aspose DotNetNuke Module Development Template</h2>
<p>Aspose DotNetNuke Module Development Template provides a quick and easy way to use a module development template for DotNetNuke 7+ that can automatically download Aspose components and seamlessly add them to your DotNetNuke module project.</p>
<p>Aspose DotNetNuke Module Development Template includes the following features:</p>
<ul>
<li>Supports Visual Studio 2010 and Visual Studio 2012. </li><li>Create DotNetNuke modules for C# or VB.NET. </li><li>Create DotNetNuke modules with the new DAL2 data access layer available in DotNetNuke 7 for C# or VB.NET.
</li><li>Select one or more Aspose components during module creation. </li><li>The latest version of the selected Aspose components is automatically downloaded and referenced.
</li></ul>
<h2>How to Install the Aspose DotNetNuke Project Templates</h2>
<p>Installing Aspose DotNetNuke project templates is pretty easy. There are multiple ways to install: choose one of these options below.</p>
<h3>Install manually by downloading the VSIX file from the DNN Store</h3>
<ol>
<li>Visit the DNN Store &ndash; <a href="http://store.dnnsoftware.com/home/product-details/aspose-net-module-development-template-for-dnn">
Aspose DotNetNuke Module Development Template page</a> </li><li>Click ‘Add to Cart’ and then proceed to checkout to download the VSIX file
</li><li>Double click on the downloaded file to install the templates. </li></ol>
<h3>Install manually by downloading from Codeplex</h3>
<ol>
<li><a href="https://asposednn.codeplex.com/">Download the VSIX file</a> from Codeplex
</li><li>Double click on the downloaded file to install the templates. </li></ol>
<p><strong>Note:</strong> Please make sure to restart Visual Studio for the changes to take effect.</p>
<h2>Creating a DotNetNuke Module using the Templates</h2>
<p>Once you’ve installed the templates, you can set up a project based on them. To do so you should follow the steps below. Before you can use them, you should have installed Visual Studio 2010 or 2012, and Aspose DotNetNuke Project Templates (described
 above).</p>
<ol>
<li>Set up the DotNetNuke Development environment following the steps in the Wiki (the templates assume you have your development environment set up at http://dnndev.me/).
<a target="_blank" href="http://www.dnnsoftware.com/Resources/Wiki/page/development-environment.aspx" title="Development Environment">
Development Environment</a> </li><li>Run Visual Studio 2010 or Visual Studio 2012 as an Administrator (right-click&nbsp; the desk-top shortcut to do so)
</li><li>From the <strong>File</strong> menu, select <strong>New Project</strong>. </li><li>Choose either C# or VB.Net from the Languages section of the new project dialog.
<div style="width:310px" id="attachment_11093"><a href="http://www.aspose.com/blogs/wp-content/uploads/2013/09/02-create-project.png"><img width="300" height="193" alt="02 create project 300x193 Aspose DotNetNuke Module Development Template Released" src="http://www.aspose.com/blogs/wp-content/uploads/2013/09/02-create-project-300x193.png" title="02-dnn-create-project"></a>
<p>Aspose DotNetNuke create project</p>
</div>
</li><li>Select the DotNetNuke Folder under your preferred language (C# or Visual Basic).
</li><li>Choose either the <strong>Aspose DotNetNuke C# Compiled Module</strong> or <strong>
Aspose DotNetNuke 7 C# DAL2 Compiled Module</strong> template for your project template (or the VB.NET versions).
</li><li>For the new project creation screen using the following settings
<ol style="list-style:lower-alpha outside none">
<li>Name: ModuleName <br>
Something unique here, example DNNTaskManager </li><li>Location: c:\websites\dnndev.me\desktopmodules\ <br>
This assumes you set up your development environment as instructed in step 1. </li><li>Solution: Create new solution </li><li>Create directory for solution : <strong>Unchecked</strong> <br>
If checked, this option will cause path problems. The templates assume that the SLN is in the same folder as the project file.
</li><li>Click <strong>OK</strong>. </li></ol>
<p>A screen is shown containing all Aspose components (screenshot below).</p>
</li><li>Select one or more components from the list. <br>
Each component’s common uses is shown upon selection. </li><li>Click <strong>Next</strong> to continue once done.
<p>&nbsp;</p>
<div style="width:568px" id="attachment_11094"><a href="http://www.aspose.com/blogs/wp-content/uploads/2013/09/03-dnn-wizard-01.png"><img width="558" height="445" alt="03 dnn wizard 01 Aspose DotNetNuke Module Development Template Released" src="http://www.aspose.com/blogs/wp-content/uploads/2013/09/03-dnn-wizard-01.png" title="03-dnn-wizard-01"></a>
<p>Aspose DotNetNuke create project select components</p>
</div>
<p>The Next screen shows the download progress for each selected component.</p>
<div style="width:568px" id="attachment_11095"><a href="http://www.aspose.com/blogs/wp-content/uploads/2013/09/04-ddn-wizard-02.png"><img width="558" height="445" alt="04 ddn wizard 02 Aspose DotNetNuke Module Development Template Released" src="http://www.aspose.com/blogs/wp-content/uploads/2013/09/04-ddn-wizard-02.png" title="04-ddn-wizard-02"></a>
<p>Aspose DotNetNuke create project component download</p>
</div>
<p>Once the downloading is completed the module is created with the selection components automatically reference.</p>
<div style="width:660px" id="attachment_11096"><a href="http://www.aspose.com/blogs/wp-content/uploads/2013/09/05-dnn-created-module.png"><img width="650" height="189" alt="05 dnn created module Aspose DotNetNuke Module Development Template Released" src="http://www.aspose.com/blogs/wp-content/uploads/2013/09/05-dnn-created-module.png" title="05-dnn-created-module"></a>
<p>Aspose DotNetNuke created module</p>
</div>
<p>This creates a folder under c:\websites\dnndev.me\desktopmodules\ModuleName which should contain all the files necessary for your module, including the solution.</p>
</li></ol>
<p>There are a couple of final steps just to finalize the process, documented in the
<strong>Documentation\Documentation.html</strong> file, which should open up automatically in Visual Studio after the project is created. Follow the steps to configure the final project properties and then you are ready to build/deploy a module. You can delete
 the documentation folder once you have completed those steps.</p>
<h2>Video</h2>
<p>Please check the video below to see it in action.</p>
<p><a href="https://www.youtube.com/watch?v=emvDF3Kqj0E">https://www.youtube.com/watch?v=emvDF3Kqj0E</a></p>
</div>