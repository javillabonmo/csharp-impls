import { argv } from "node:process";
import { crawlPage } from "./crawl.ts";

const args = argv.slice(2);

if (args.length < 1) {
    console.error("Error: expected exactly one argument (the base URL)");
    process.exit(1);
}

if (args.length > 1) {
    console.error(`Error: expected exactly one argument, got ${args.length}`);
    process.exit(1);
}

const baseURL = args[0] as string;
console.log(`Crawler starting at ${baseURL}`);

const pages = await crawlPage(baseURL, baseURL);
