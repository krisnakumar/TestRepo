##

##

#
## Setup Guide for LMS Report Builder App Installation &amp; Configuration

#

**Author: Krishna Kumar, Tech Lead**

**Document History**

| **Version** | **Author** | **Date** | **Comment** |
| --- | --- | --- | --- |
| 0.1 | Krishna Kumar | 01/30/2019 |   |
| 0.2 | Arul Deepan | 01/30/2019 | Updated the Deployment section |
| 0.3 | Prashanth Ravi | 01/31/2019 | Updated the Setup Prerequisites section |


## 1. Introduction

The document defines the Set up procedure for Report Builder Web application. This includes setting up the environment for development, monitoring, support and maintenance of Web.

1.1 Software Architecture Diagrams

Please click to see the [ITS - LMS - ReportBuilder - SAD Diagram](https://docs.google.com/drawings/d/15O8ASUS5nqJZAbDknaJID5_2IaQhGk2B88W4ojXNXzU).

1.2.  Environments

1.2.1 S3 Bucket

| **Environment** | **Bucket Name** |
| --- | --- |
| Dev | \*\* DEV BUCKET NAME HERE \*\* |
| QA | \*\* QA BUCKET NAME HERE \*\* |
| PROD | \*\* PROD BUCKET NAME HERE \*\* |

## 2. Software Components

| **Software Component** | **Name of the Component** |
| --- | --- |
| Web Layer | React.Js |

## 3.  Setup Prerequisites

3.1. Pre-requisites

3.1.1. Hardware Prerequisites

| **Parameter** | **Requirement** |
| --- | --- |
| Operating System | Windows/Mac |
| Memory | Minimum of 8 GB |
| Hard drive | Minimum of 80 GB |

3.1.2. Software Prerequisites

| **Software** | **Version** |
| --- | --- |
| Visual Studio Code | 1.30 |
| Node js | 11.8.0 |
| React js | 16.7 |
| NPM | 6.7.0 |

3.2.  Installation

3.2.1. Install Node and NPM

1. **1.**** Install Node.js runtime and npm package manager**
  1. Go to [js](https://nodejs.org/en/download/) official site
  2. Click on the appropriate installer link based on your operating system.
  3. Once downloaded, run the msi to install Node.js
  4. Follow the prompts in the installer (Accept the license agreement, click the NEXT button a bunch of times and accept the default installation settings).
  5. Restart your computer. You won&#39;t be able to run Node.js until you restart your computer.

1. **2.**** Make sure you have Node and NPM installed by running simple commands to see what version of each is installed and to run a simple test program**
  1. **Test Node:** To see if Node is installed, open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool, and type node -v. This should print a version number, so you&#39;ll see something like this v0.11.80.
  2. **Test NPM:** To see if NPM is installed, type npm -v in Terminal. This should print NPM&#39;s version number so you&#39;ll see something like this 6.7.0
  3. **Create a test file and run it:** A simple way to test that node.js works is to create a JavaScript file: name it hello.js, and just add the code console.log(&#39;Node is installed!&#39;);. To run the code simply open your command line program, navigate to the folder where you save the file and type node hello.js. This will start Node and run the code in the hello.js file. You should see the output Node is installed!.

**Note:** To Installing Node.js via package manager follow the steps on the following [site](https://nodejs.org/en/download/package-manager/)

3.3 Project Creation

3.3.1. Steps to create project

Create React App is an officially supported way to create single-page React applications. It offers a modern build setup with minimal configuration.

1. 1)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm init react-app \&lt;PROJECT-NAME\&gt;
  2. b)cd \&lt;PROJECT-NAME\&gt;
  3. c)npm run eject

**Note:**  This is an one-way operation. Once you eject, you shall not be able to go back!
If you aren&#39;t satisfied with the build tool and configuration choices, you can eject at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (Webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except eject will still work, but they will point to the copied scripts so you can tweak them. At this point you&#39;re on your own.

You don&#39;t have to ever use eject. The curated feature set is suitable for small and middle deployments, and you shouldn&#39;t feel obligated to use this feature. However we understand that this tool wouldn&#39;t be useful if you couldn&#39;t customize it when you are ready for it.

3.3.2  Install package dependencies

1. 1)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool

1.
  1. a)npm install

3.3.3  To run project in development mode

1. 1)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm start

**Note:** Runs the app in the development mode. Open [http://localhost:3000](http://localhost:3000) to view it in the browser. The page will reload if you make edits. You will also see any lint errors in the console.

3.3.4  To run unit tests

1. 2)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm test

**Note:  ** Launches the test runner in the interactive watch mode.

3.3.5  To create optimised production build

1. 3)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm run build

**Note:  ** Builds the app for production to the **build** folder. It correctly bundles React in production mode and optimizes the build for the best performance.

## 4.  Deployment

4.1 VSTS Git Branching

In Report Builder app we are having two major branches. Apart from this each individual developers will have their own branch for development purpose.

The major branches are

1. Development
2. QA
3. Master

The developers will commit their changes on a daily basis.

4.2. Continuous Integration Tool Process

It is common on software projects for developers to work in parallel. At some point, it is necessary to integrate all of these parallel streams of work into one codebase that makes up the final product. In the early days of software development, this integration was performed at the end of a project, which was a difficult and risky process.

Continuous Integration (CI) avoid such complexities by merging every developer&#39;s changes into the common code base on a continual basis. Continuous Integration is a software engineering practice in which developer will be check-in their daily work to the centralized repository management system from where an automated build compiles and optionally tests an app when code is added or changed by developers in the project&#39;s version control repository.

In ITS LMS Data Integration App we are using VSTS Git as a repository management system. Where as the build and release pipeline will be handled in VSTS (Azure DevOps)

4.2.1 Visual Studio Team Services.

Visual studio team services is a Saas (Software as a service) which allow us to collaborate with the team and provide couple of other services for repository management and Build and release management.

This guide shows how to **use VSTS** and configure it to run automated builds and release pipeline. Once we setup the VSTS,  then additional plugins need to be installed to support MS Build. By default, VSTS supports **VSTS**** Git.** If TFS is being used for source code control, VSTS as well provide option to integrate it with the build and release pipeline.

Once VSTS is setup and any necessary plugins have been installed, we will create user roles and permission necessary to access the VSTS services. Up on that we will create one or more jobs to compile the Data Integration application A job is a collection of steps and metadata required to perform some work. A job typically consists of the following:

- **Agent -** To build your code or deploy your software you need at least one agent. An agent is installable software that runs one build or deployment job at a time.
- **Source code Checkout** – This is a metadata entry in the VSTS configuration that contains information on how to connect to source code control and what files to retrieve.
- **Triggers** – Triggers are used to start a job based on certain actions, such as when a developer commits changes to the source code repository.
- **Versioning** - This is an extension that will automatically setup the build version based on the criteria.
- **Package restore** -  This is again an extension that will automatically restore the necessary packages that are required to build the data integration application.
- **Build Instructions** – This is an extension or a script that will compile the source code and produce a binary artifact that can be installed on mobile devices.
- **Certificates -** This is an extension which will handle the certificate installation for IOS and android build.
- **Release Notes -** This is an extension that will generate the release notes by comparing the previous commit and current commit.
- **Notifications** – An extension that will send out some kind of notification about the status of a build.
- **Security** – Although optional, it is strongly recommended that the VSTS security features also be enabled.



4.2.2 Installing Extensions

Extensions enhance Azure DevOps Services and Team Foundation Server (TFS) by contributing enhancements like new web experiences, dashboard widgets, build tasks, and more. The extensions provide the integration of Build pipeline with third party softwares and services. The extensions can be installed from the Marketplace.





From the above page, In the top right corner. You can choose the Browse marketplace Button and you can reach the Marketplace.

 

The above is the home page for Azure DevOps Marketplace.  Here, you can search for the required extension and you can add them to your organisation&#39;s VSTS.

4.3 Setting up the Report Builder React Build and Release Job for QA

At the top level, VSTS organizes all of the various tasks required to build software into a job. A job also has metadata associated with it, providing information about the build such as how to get the source code, how often the build should run, any special variables that are necessary for building, and how to notify developers if the build fails.

In VSTS, the CI/CD has two steps process as mentioned below;

1. Build Pipeline
2. Release Pipeline

4.4 Setup the Build pipeline for Report Builder API QA Branch

Jobs are created by selecting **Build and release \&gt; New \&gt; New build pipeline** from the menu in the upper left side, as shown in the following screenshot:

 

This will display the New Job page. Select the source from where the job has to check-out the code. Choose the repository and choose the branch. Then choose continue.

 
VSTS will ask you to choose the template. You can choose this based on the project. If you didn&#39;t have any extensions pre installed you can choose empty job and create it.

 

Once the initial job has been created, it must be configured with one or more of the following:

- The source code management system must be specified.
- One or more build actions must be added to the project. These are the steps or tasks required to build the application.
- The job must be assigned one build trigger – a set of instructions informing Jenkins how often to retrieve the code and build the final project.

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| Pipeline | **Report Builder API** | This is the build pipeline name for the Report Builder API application. |
| Agent Pool | **Hosted VS2017**   | When we queue a build, it executes on an agent from the selected pool. we have selected the Hosted VS2017 for Android application. |

4.4.1 Version Assemblies

This extension updates the version name of the assemblies to match the build number

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| Version | **2.\*** |   |
| Source Path | **OnBoardConnect\_Droid/Properties** | Path in which to search for version files (like AssemblyInfo.\* files). Leave empty to use the sources directory. NOTE: this is case sensitive for non-Windows systems. |
| File Pattern | **package.json** | File filter to replace version info. |
| Version Source | **Build Number** | The source for the version number. Defaults to the build number, but can be a build variable. |
| Version Extract Pattern | **1.0.0.0** | Pattern to use to extract the build number as well as default replacement pattern. |
| Under Advanced-\&gt; Replace Pattern | **Custom Regex** | Pattern to use to replace the version in the files. |
| Custom Regex Replace Pattern | **&quot;version&quot;: &quot;\d+\.\d+\.\d+\.\d+** | Regular Expression to replace with in files. |
| Build Regex Group Index | **0** | Index of group in build regular expression that you want to use as the version number. Leave as 0 if you have no groups. |
| Prefix for Replacements | **&quot;version&quot;: &quot;** | String to prefix the regex replacement with. |
| Postfix for Replacements | **-** | String to postfix the regex replacement with. |

4.4.2 NPM Install

To create a package for deployment of a React Js application, we first add the npm install extension to our build pipeline.

 

After we add the task, We need to configure the task to build and generate the artifacts.  This extension will install the packages for the application.

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| Version | **1.\*** |   |
| Command | **Install** | The command and arguments which will be passed to npm for execution. |

4.4.3 NPM Build

This task will build and publish npm packages, or run an npm command. Supports npmjs.com and authenticated registries like Package Management.

 
| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| Version | 1\* |   |
| Command | Custom | The command and arguments which will be passed to npm for execution |
| Command and arguments | run build | Custom command to run, e.g. &quot;dist-tag ls mypackage&quot;. |

4.4.4 Publish Artifact

We do the Artifact Publishing using the VSTS-provided Publish Build Artifacts task. I&#39;ve set the logical name of the artifact to be Packaged Function (some users might also use &quot;drop&quot; to match VSTS terminology). We&#39;ll use the artifact name when we add the release pipeline to link the artifacts from our build pipeline into the release pipeline. Path to Publish must match the output from our React Js application package _$(System.DefaultWorkingDirectory)/build/_

 

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| Version | **1.\*** |   |
| Path to publish | **$(System.DefaultWorkingDirectory)/build/** | The folder or file path to publish. This can be a fully-qualified path or a path relative to the root of the repository.   |
| Artifact name | **reportbuilder-fe** | The name of the artifact to create in the publish location. |
| Artifact publish location | **Azure Pipelines/TFS** | Choose whether to store the artifact in Azure Pipelines/TFS, or to copy it to a file share that must be accessible from the build agent. |

4.5 Location of Build Artifacts

The location of the build artifact is controlled in the source code. We can further modify the source code based on our need.

4.6 Release Pipeline

4.6.1 S3 Upload: its-report-builder

In our release pipeline this time we use the [AWS CloudFormation Create/Update Stack](https://docs.aws.amazon.com/vsts/latest/userguide/cloudformation-create-update.html) task because we need to upload the packages to the S3 hosting bucket. This task will Upload file and folder content to an Amazon Simple Storage Service (S3) Bucket.

This task requires permissions to call the following AWS service APIs (depending on selected task options, not all APIs may be used):

s3:CreateBucket

s3:HeadBucket

Content uploads are performed using S3&#39;s PutObject API and/or the multi-part upload APIs. The specific APIs used depend on the size of the individual files being uploaded.

 

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| **Version** | **1.\*** |   |
| AWS Credentials | **ITS-CI-User** | Specifies the AWS credentials to be used by the task in the build agent environment. |
| AWS Region | **US West (Oregon) [us-west-2]** | The AWS region code (us-east-1, us-west-2 etc) of the region containing the AWS resource(s) the task will use or create |
| Bucket Name | **its-report-builder** | The name of the Amazon S3 bucket to which the content will be uploaded. If the bucket does not exist it can be created if the _Create S3 bucket if it does not exist_ option is selected. |
| Source Folder | **$(System.DefaultWorkingDirectory)/\_Report Builder FE/reportbuilder-fe** | The source folder that the filename selection pattern(s) will be run against. If not set the root of the work area is assumed. |
| Filename Patterns | **\*\*** | Glob patterns to select the file and folder content to be uploaded. Supports multiple lines of minimatch patterns. |
| Access Control (ACL) | **public read** | The canned Access Control List (ACL) to apply to the uploaded content |
| Create S3 bucket if it does not exist | **Enable this checkbox** | Attempts to automatically create the S3 bucket if it does not exist. |

4.6.2 Generate Release Notes

This task helps you to generate the release notes automatically for the release.

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| **Version** | **2.\*** |   |
| Output file | **releasenotes.htm** | The name of the Markdown file to export e.g. $(Build.ArtifactStagingDirectory)\releasenotes.md if within a build workflow |
| Template Location | **InLine** | Select the template file source, file in source control or in line. |
| Template | **\&lt;h2\&gt;Onboard LMS ReportBuilder Frontend Deployment Success\&lt;/h2\&gt;**** \&lt;h2\&gt;Release notes for release $($build.buildnumber) \&lt;/h2\&gt; ** ** @@BUILDLOOP@@ ****\&lt;h3\&gt;Associated work items  \&lt;/h3\&gt;**** @@WILOOP@@  ****\&lt;li\&gt; \&lt;b\&gt;\&lt;a href=&quot;https://its-training.visualstudio.com/OnBoardLMS%20Sysvine/\_workitems/edit/$($widetail.id)&quot;\&gt;$($widetail.fields.&#39;System.WorkItemType&#39;) $($widetail.id)\&lt;/a\&gt; \&lt;/b\&gt; - $($widetail.fields.&#39;System.Title&#39;)  ****@@WILOOP@@   ****@@BUILDLOOP@@** | The Markdown template. |
| Output variable | **releasenotes** |   |

4.6.3 Send email

This task help you to send out the release notes to the developers and QA through email. This use a SMTP email settings to send our the email notifications.

| **Parameter** | **Value** | **Comments** |
| --- | --- | --- |
| Version | **1.\*** |   |
| To Addresses | **jjeeva@its-training.com** | To Addresses. Separate by semicolon (;) |
| CC Addresses | **mraj@its-training.com; hprasad@its-training.com; kkumar@its-training.com; aravi@its-training.com; seswar@its-training.com; adeepan@its-training.com; amunoz@its-training.com; bstinson@its-training.com** | CC Addresses. Separate by semicolon (;) |
| BCC Addresses | **pravi@its-training.com** | BCC Addresses. Separate by semicolon (;) |
| From Address | **notifications@its-training.com** | Address from which the email is sent |
| Mail Subject | **Onboard LMS Report Builder Lambda Functions Deployment Success** | The subject of the email |
| Mail Body | **$(releasenotes)** | The body of the email |
| Is HTML Body?: | **Enable this checkbox** | Indicate if the text in the Body is HTML formatted |
| SMTP Server | **smtp-mail.outlook.com** | Name or IP Address of a SMTP server |
| SMTP Port | **587** | Port to the SMTP server |
| SMTP Username | **notifications@its-training.com** | Username for the SMTP server |
| SMTP Password | **$(smtppassword)** | Password for the SMTP server |
| SMTP Use SSL? | **Enable this checkbox** | SMTP Use SSL? |

4.7 Variables

This section will consist of list of user defined variables we have used to configure the Onboard Connect Droid build pipeline.

| **Variable Name** | **Value** |
| --- | --- |
| smtp.password | **\*\*\*\*\*\*\*\*\*** |
| version | **5.0 (Will be updated based on the app version)** |

4.8 Build Triggers

There are several different strategies for initiating builds in VSTS – these are known as build triggers. A build trigger helps VSTS decide when to start a job and build the project. Three of the more common build triggers are:

- **Build periodically** – This trigger causes VSTS to start a job at specified intervals, such as every two hours or at midnight on weekdays. The build will start regardless of whether there have been any changes in the source code repository.
- **Poll SCM** – This trigger will poll source code control on a regular basis. If any changes have been committed to the source code repository, VSTS will start a new build.
- Build Manually - This is obviously the manual process of triggering the build. We can trigger the build manually based on the business requirements.

Polling SCM is a popular trigger because it provides quick feedback when a developer commits changes that cause the build to break. This is useful for alerting teams that some recently committed code is causing problems, and lets the developers address the problem while the changes are still fresh in mind.

Periodic builds are often used to create a version of the application that can be distributed to testers. For example, a periodic build might be scheduled for Friday evening so that members of the QE team can test the work of the previous week.

5  Monitoring

5.7.1. Log Location

| **Environment** | **Log Path** |
| --- | --- |
| QA | [https://us-west-2.console.aws.amazon.com/cloudwatch/home?region=us-west-2#logs](https://us-west-2.console.aws.amazon.com/cloudwatch/home?region=us-west-2#logs:) |

5.7.2 Reactive Monitoring

        We will check the logs weekly to see the Application crashes, performance and other issues.

5.7.3 Proactive Monitoring

Currently not available, we will introduce this in going forward