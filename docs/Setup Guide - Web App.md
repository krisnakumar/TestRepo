###

###

##
### Setup Guide for LMS Report Builder App Installation &amp; Configuration

##

**Author: Krishna Kumar, Tech Lead**

**Document History**

| **Version** | **Author** | **Date** | **Comment** |
| --- | --- | --- | --- |
| 0.1 | Krishna Kumar | 01/30/2019 |   |
| 0.2 | Arul Deepan | 01/30/2019 | Updated the Deployment section |
| 0.3 | Prashanth Ravi | 01/31/2019 | Updated the Setup Prerequisites section |
| 0.4 | Prashanth Ravi | 05/22/2019 | Updated VSTS Git branching information |
| 0.5 | Hari Prasad | 05/23/2019 | Reviewed and shared feedback on revisions to be made |

**Table of Contents**

[1. Introduction￼](bookmark://_Toc9472852#_Toc9472852)[        5](bookmark://_Toc9472852#_Toc9472852)

[1.1 Software Architecture Diagram￼](bookmark://_Toc9472853#_Toc9472853)[        5](bookmark://_Toc9472853#_Toc9472853)

[1.2.  Environments￼](bookmark://_Toc9472854#_Toc9472854)[        5](bookmark://_Toc9472854#_Toc9472854)

[1.2.1 S3 Bucket￼](bookmark://_Toc9472855#_Toc9472855)[        5](bookmark://_Toc9472855#_Toc9472855)

[2. Software Components￼](bookmark://_Toc9472856#_Toc9472856)[        5](bookmark://_Toc9472856#_Toc9472856)

[3.  Setup Prerequisites￼](bookmark://_Toc9472857#_Toc9472857)[        5](bookmark://_Toc9472857#_Toc9472857)

[3.1. Pre-requisites￼](bookmark://_Toc9472858#_Toc9472858)[        5](bookmark://_Toc9472858#_Toc9472858)

[3.1.1. Hardware Prerequisites￼](bookmark://_Toc9472859#_Toc9472859)[        5](bookmark://_Toc9472859#_Toc9472859)

[3.1.2. Software Prerequisites￼](bookmark://_Toc9472860#_Toc9472860)[        6](bookmark://_Toc9472860#_Toc9472860)

[3.2.  Installation￼](bookmark://_Toc9472861#_Toc9472861)[        6](bookmark://_Toc9472861#_Toc9472861)

[3.2.1. Install Node and NPM￼](bookmark://_Toc9472862#_Toc9472862)[        6](bookmark://_Toc9472862#_Toc9472862)

[3.3 Project Creation￼](bookmark://_Toc9472863#_Toc9472863)[        7](bookmark://_Toc9472863#_Toc9472863)

[3.3.1. Steps to create project￼](bookmark://_Toc9472864#_Toc9472864)[        7](bookmark://_Toc9472864#_Toc9472864)

[3.3.2  Install package dependencies￼](bookmark://_Toc9472865#_Toc9472865)[        7](bookmark://_Toc9472865#_Toc9472865)

[3.3.3  To run project in development mode￼](bookmark://_Toc9472866#_Toc9472866)[        8](bookmark://_Toc9472866#_Toc9472866)

[3.3.4  To run unit tests￼](bookmark://_Toc9472867#_Toc9472867)[        8](bookmark://_Toc9472867#_Toc9472867)

[3.3.5  To create optimized production build￼](bookmark://_Toc9472868#_Toc9472868)[        8](bookmark://_Toc9472868#_Toc9472868)

[4.  Development￼](bookmark://_Toc9472869#_Toc9472869)[        8](bookmark://_Toc9472869#_Toc9472869)

[4.1 VSTS Git Branching￼](bookmark://_Toc9472870#_Toc9472870)[        8](bookmark://_Toc9472870#_Toc9472870)

[4.2. Cloning repository￼](bookmark://_Toc9472871#_Toc9472871)[        9](bookmark://_Toc9472871#_Toc9472871)

[4.3. Setup local development environment￼](bookmark://_Toc9472872#_Toc9472872)[        9](bookmark://_Toc9472872#_Toc9472872)

[4.4. Updating the remote origin￼](bookmark://_Toc9472873#_Toc9472873)[        11](bookmark://_Toc9472873#_Toc9472873)

[5. Deployment￼](bookmark://_Toc9472874#_Toc9472874)[        11](bookmark://_Toc9472874#_Toc9472874)

[5.1 VSTS Git Branching￼](bookmark://_Toc9472875#_Toc9472875)[        11](bookmark://_Toc9472875#_Toc9472875)

[5.2. Continuous Integration Tool Process￼](bookmark://_Toc9472876#_Toc9472876)[        12](bookmark://_Toc9472876#_Toc9472876)

[5.2.1 Visual Studio Team Services.￼](bookmark://_Toc9472877#_Toc9472877)[        12](bookmark://_Toc9472877#_Toc9472877)

[5.2.2 Installing Extensions￼](bookmark://_Toc9472878#_Toc9472878)[        14](bookmark://_Toc9472878#_Toc9472878)

[5.3 Setting up the Report Builder React Build and Release Job for QA￼](bookmark://_Toc9472879#_Toc9472879)[        15](bookmark://_Toc9472879#_Toc9472879)

[5.4 Setup the Build pipeline for Report Builder API QA Branch￼](bookmark://_Toc9472880#_Toc9472880)[        16](bookmark://_Toc9472880#_Toc9472880)

[5.4.1 Version Assemblies￼](bookmark://_Toc9472881#_Toc9472881)[        18](bookmark://_Toc9472881#_Toc9472881)

[5.4.2 NPM Install￼](bookmark://_Toc9472882#_Toc9472882)[        19](bookmark://_Toc9472882#_Toc9472882)

[5.4.3 NPM Build￼](bookmark://_Toc9472883#_Toc9472883)[        20](bookmark://_Toc9472883#_Toc9472883)

[5.4.4 Publish Artifact￼](bookmark://_Toc9472884#_Toc9472884)[        20](bookmark://_Toc9472884#_Toc9472884)

[5.5 Location of Build Artifacts￼](bookmark://_Toc9472885#_Toc9472885)[        21](bookmark://_Toc9472885#_Toc9472885)

[5.6 Release Pipeline￼](bookmark://_Toc9472886#_Toc9472886)[        21](bookmark://_Toc9472886#_Toc9472886)

[5.6.1 S3 Upload: its-report-builder￼](bookmark://_Toc9472887#_Toc9472887)[        22](bookmark://_Toc9472887#_Toc9472887)

[5.6.2 Generate Release Notes￼](bookmark://_Toc9472888#_Toc9472888)[        23](bookmark://_Toc9472888#_Toc9472888)

[5.6.3 Send email￼](bookmark://_Toc9472889#_Toc9472889)[        23](bookmark://_Toc9472889#_Toc9472889)

[5.7 Variables￼](bookmark://_Toc9472890#_Toc9472890)[        24](bookmark://_Toc9472890#_Toc9472890)

[5.8 Build Triggers￼](bookmark://_Toc9472891#_Toc9472891)[        25](bookmark://_Toc9472891#_Toc9472891)

[6. Monitoring￼](bookmark://_Toc9472892#_Toc9472892)[        25](bookmark://_Toc9472892#_Toc9472892)

[6.1 Log Location￼](bookmark://_Toc9472893#_Toc9472893)[        25](bookmark://_Toc9472893#_Toc9472893)

[6.2 Reactive Monitoring￼](bookmark://_Toc9472894#_Toc9472894)[        26](bookmark://_Toc9472894#_Toc9472894)

[6.3 Proactive Monitoring￼](bookmark://_Toc9472895#_Toc9472895)[        26](bookmark://_Toc9472895#_Toc9472895)

#
### 1. Introduction

The document defines the Set up procedure for Report Builder Web application. This includes setting up the environment for development, monitoring, support and maintenance of Web.

## 1.1 Software Architecture Diagram

Please click to see the [ITS - LMS - ReportBuilder - SAD Diagram](https://docs.google.com/drawings/d/15O8ASUS5nqJZAbDknaJID5_2IaQhGk2B88W4ojXNXzU).

## 1.2.  Environments

### 1.2.1 S3 Bucket

| **Environment** | **Bucket Name** |
| --- | --- |
| Dev |

#
[ANNOTATION:

BY &#39;Prashanth Ravi&#39;
ON &#39;2019-05-23T15:23:00&#39;PR
NOTE: &#39;@Arul Deepan Update the S3 bucket details&#39;]

#
[ANNOTATION:

BY &#39;Hari Prasad&#39;
ON &#39;2019-05-22T16:32:00&#39;HP
NOTE: &#39;Yet to be updated?&#39;]
\*\* DEV BUCKET NAME HERE \*\*
 |
| PROD | \*\* PROD BUCKET NAME HERE \*\* |

#
### 2. Software Components

| **Software Component** | **Name of the Component** |
| --- | --- |
| Web Layer | React.Js |

#
### 3.  Setup Prerequisites

## 3.1. Pre-requisites

### 3.1.1. Hardware Prerequisites

| **Parameter** | **Requirement** |
| --- | --- |
| Operating System | Windows/Mac |
| Memory | Minimum of 8 GB |
| Hard drive | Minimum of 80 GB |

### 3.1.2. Software Prerequisites

| **Software** | **Version** |
| --- | --- |
| Git | 2.6.4  or above |
| Visual Studio Code | 1.30 or above |
| Node js | 10.0.0 or above |
| React js | 16.5 or above |
| NPM | 6.7.0 or above |

## 3.2.  Installation

### 3.2.1. Install Node and NPM

1. **1.**** Install Node.js runtime and npm package manager**
  1. Go to [js](https://nodejs.org/en/download/) official site
  2. Click on the appropriate installer link based on your operating system.
  3. Once downloaded, run the msi to install Node.js
  4. Follow the prompts in the installer (Accept the license agreement, click the NEXT button a bunch of times and accept the default installation settings).
  5. Restart your computer. You won&#39;t be able to run Node.js until you restart your computer.

2. **2.**** Make sure you have Node and NPM installed by running simple commands to see what version of each is installed and to run a simple test program**
  6. **Test Node:** To see if Node is installed, open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool, and type node -v. This should print a version number, so you&#39;ll see something like this v0.11.80.
  7. **Test NPM:** To see if NPM is installed, type npm -v in Terminal. This should print NPM&#39;s version number so you&#39;ll see something like this 6.7.0
  8. **Create a test file and run it:** A simple way to test that node.js works is to create a JavaScript file: name it hello.js, and just add the code console.log(&#39;Node is installed!&#39;);. To run the code simply open your command line program, navigate to the folder where you save the file and type node hello.js. This will start Node and run the code in the hello.js file. You should see the output Node is installed!.

**Note:** To Installing Node.js via package manager, follow the steps in this [site](https://nodejs.org/en/download/package-manager/)[.](https://nodejs.org/en/download/package-manager/)

URL: [https://nodejs.org/en/download/package-manager/](https://nodejs.org/en/download/package-manager/)

## 3.3 Project Creation

### 3.3.1. Steps to create project

Create React App is an officially supported way to create single-page React applications. It offers a modern build setup with minimal configuration.

Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool

1.
1)npm init react-app \&lt;
#
[ANNOTATION:

BY &#39;Hari Prasad&#39;
ON &#39;2019-05-23T15:08:00&#39;HP
NOTE: &#39;Should this be Report Builder or LMS?&#39;]
PROJECT-NAME\&gt;
2. 2)cd \&lt;PROJECT-NAME\&gt;
3. 3)npm run eject

**Note:**  This is an one-way operation. Once you eject, you shall not be able to go back! If you aren&#39;t satisfied with the build tool and configuration choices, you can eject at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (Webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except eject will still work, but they will point to the copied scripts so you can tweak them. At this point you&#39;re on your own.

You don&#39;t have to ever use eject. The curated feature set is suitable for small and middle deployments, and you shouldn&#39;t feel obligated to use this feature. However we understand that this tool wouldn&#39;t be useful if you couldn&#39;t customize it when you are ready for it.

### 3.3.2  Install package dependencies

1. 1)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool

1.
  1. a)npm install

### 3.3.3  To run project in development mode

1. 1)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm start

**Note:** Runs the app in the development mode. Open [http://localhost:3000](http://localhost:3000) to view it in the browser. The page will reload if you make edits. You will also see any lint errors in the console.

### 3.3.4  To run unit tests

1. 2)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm test

**Note:  ** Launches the test runner in the interactive watch mode.

### 3.3.5  To create optimized production build

1. 3)Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool
  1. a)npm run build

**Note:  ** Builds the app for production to the **build** folder. It correctly bundles React in production mode and optimizes the build for the best performance.

#
### 4.  Development

## 4.1 VSTS Git Branching

VSTS Git is optimized to facilitate collaborative development of software, but it has storage and version control capabilities that may be similarly applied to the management of digital deliverables in a preservation system. In this mode of usage, each digital object would be stored in its own Git &quot;repository&quot; and standard Git commands would be used to add or update the object&#39;s content and metadata files. The Git clone and pull commands could be used for replication to additional storage locations.



In VSTS Git, the repo holds two main branches.

- Master
- Dev

In Report Builder app there are individual developers, each having their own dedicated branch for development apart from above two major branches. The developers will commit their changes on a daily basis.

| **Project Name** | **Repository Name** | **Repository URL** |
| --- | --- | --- |
|
[OnBoardLMS Sysvine](https://its-training.visualstudio.com/OnBoardLMS%20Sysvine) |
[ReportBuilderFrontEnd](https://its-training.visualstudio.com/OnBoardLMS%20Sysvine/_git/ReportBuilderFrontEnd)
 | [https://its-training.visualstudio.com/OnBoardLMS%20Sysvine/\_git/ReportBuilderFrontEnd](https://its-training.visualstudio.com/OnBoardLMS%20Sysvine/_git/ReportBuilderFrontEnd) |

## 4.2. Cloning repository

1.
1.
#
[ANNOTATION:

BY &#39;Prashanth Ravi&#39;
ON &#39;2019-05-23T15:21:00&#39;PR
NOTE: &#39;I have fixed the numberings&#39;]

#
[ANNOTATION:

BY &#39;Hari Prasad&#39;
ON &#39;2019-05-23T15:15:00&#39;HP
NOTE: &#39;The number starts from 03, Is this a typo or are we missing anything?&#39;]
Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool

1.
  1. Navigate to folder using above tools where the repo is to be cloned
  2. Make sure you have Git installed by running simple command on your command-line tool to see what version is installed
    1. git --version
  3. In your command-line tool
    1. gitclone [https://its-training.visualstudio.com/OnBoardLMS%20Sysvine/\_git/ReportBuilderFrontEnd](https://its-training.visualstudio.com/OnBoardLMS%20Sysvine/_git/ReportBuilderFrontEnd)
  4. Enter your Git credentials
  5. Once the cloning is completed, checkout to desired branch by following steps
  6. To fetch a branch, you simply need to:
    1. gitfetch origin
  7. This will fetch all of the remote branches for you. You can see the branches available for checkout with
    1. git branch -v -a
  8. Checkout the to desired branch
    1. git checkout dev/pravi

## 4.3. Setup local development environment

In Report Builder app currently we are having four different Apps for frond end. Which all are act as separate standalone React Applications

The different Apps are

1. Workbook Dashboard
2. Query Builder
3. COQ Dashboard
4. Training Dashboard



| **Application Name** | **Repository Folder** | **Application Router basename** |
| --- | --- | --- |
| Workbook Dashboard | workbooks-dashboard | /training/ojt/dashboard |
| Query Builder | query-builder | /reports/query-builder |
| OQ Dashboard | contractor-oq-dashboard | /contractor-management/reports/oq-dashboard |
| Training Dashboard | contractor-training-dashboard | /contractor-management/reports/training-dashboard |



To run the App in local development environment, Open the Windows Command Prompt, Powershell, Terminal  or a similar command line tool

Install package dependencies

npm install

To run project in development mode

npm start

**Note:** Runs the app in the development mode. Open [http://localhost:3000](http://localhost:3000/) to view it in the browser. The page will reload if you make edits. You will also see any lint errors in the console.

To run unit tests

npm run test

**Note:  ** Launches the test runner and generate the code coverage reports on folder called **coverage**.



To create optimised production build

npm run build

Note: We need to set the   **Application Router basename** path for each new Apps to be created on start file **src/index.jsx** as below image, **Application Router basename** is name that needs to be matched as the sub folder of S3 bucket where the App is to be hosted



    (e.g) Consider the OQ Dashboard App,

- The above app is hosted on AWS S3 in following folder structure

-
  - \&lt;AWS-S3-BUCKET-NAME\&gt;

-
  -
    - /contractor-management

-
  -
    -
      - /reports

-
  -
    -
      -
        - /oq-dashboard (Which as the build artifacts )

- Then the   **Router basename is** /contractor-management/reports/oq-dashboard

## 4.4. Updating the remote origin

We consider origin/master to be the main branch where the source code always reflects a production-ready state.

We consider origin/dev to be the main branch where the source code always reflects a state with the latest delivered development changes for the next release. This is where any automatic nightly builds are built from. The developers will commit their changes on a daily basis.

When the source code in the develop branch reaches a stable point and is ready to be released, all of the changes should be merged back into master by creating the Pull Request from dev branch to master branch by adding list of reviewers and watchers, After the confirmation of reviewers code will be merged to master branch and then tagged with a Production release Build/Version number.

# 5. Monitoring

### 5.1 Log Location

| **Environment** | **Log Path** |
| --- | --- |
| QA | [https://us-west-2.console.aws.amazon.com/cloudwatch/home?region=us-west-2#logs](https://us-west-2.console.aws.amazon.com/cloudwatch/home?region=us-west-2) |

### 5.2 Reactive Monitoring

        We will check the logs weekly to see the Application crashes, performance and other issues.

### 5.3 Proactive Monitoring

Currently not available, we will introduce this in going forward
