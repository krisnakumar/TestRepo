let fs = require("fs"),
    path = require("path"),
    inline = require("inline-css");

const TEST_RESULTS_DIRECTORY = "./";
const CODE_COVERAGE_DIRECTORY = "./coverage";

fs.readdir(CODE_COVERAGE_DIRECTORY, (err, files)=> {
    if(err) { throw new Error(err); }

    let reports = files.filter((report)=> {
        return report.endsWith(".html");
    });

    reports.forEach((report)=> {
        let filePath = path.join(CODE_COVERAGE_DIRECTORY, report);
        let options = { 
            url: "file://" + path.resolve(filePath),
            extraCss: ".pad1 { padding: 0; }"
        };

        fs.readFile(path.resolve(filePath), (err, data)=> {
            inline(data, options)
                .then((html)=> {
                    let outputFile = path.join(TEST_RESULTS_DIRECTORY, report);
                    fs.writeFile(outputFile, html, (err)=> {
                        if(err) { throw err; }
                    });
                })
                .catch((err)=> {
                    console.log(err);
                });
        });
    });
});
