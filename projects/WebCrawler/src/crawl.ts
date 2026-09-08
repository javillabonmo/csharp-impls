import { JSDOM } from 'jsdom'
import pLimit from 'p-limit'


function normalizeURL(url: string): string {
    const parsedURL = new URL(url);
    const host = parsedURL.hostname;
    const path = parsedURL.pathname.replace(/\/$/, "");
    return `${host}${path}`;
}

function getHeadingFromHTML(html: string): string {

    const dom = new JSDOM(html);
    const document = dom.window.document;
    const h1 = document.querySelector("h1");
    const h2 = document.querySelector("h2");

    if (h1) {
        return h1.textContent;
    } else if (h2) {
        return h2.textContent;
    } else {
        return "";
    }

}

function getFirstParagraphFromHTML(html: string): string {

    const dom = new JSDOM(html);
    const document = dom.window.document;
    const main = document.querySelector("main");
    if (main) {
        const p = main.querySelector("p");
        if (p) {
            return p.textContent;
        }
    }
    const p = document.querySelector("p");
    if (p) {
        return p.textContent;
    }
    return "";

}

function getURLsFromHTML(html: string, baseURL: string): string[] {
    const dom = new JSDOM(html);
    const document = dom.window.document;
    const anchors = document.querySelectorAll("a");
    const urls: string[] = [];

    anchors.forEach((anchor) => {
        const href = anchor.getAttribute("href");
        if (href) {
            urls.push(new URL(href, baseURL).toString());
        }
    });

    return urls;
}

function getImagesFromHTML(html: string, baseURL: string): string[] {
    const dom = new JSDOM(html);
    const document = dom.window.document;
    const images = document.querySelectorAll("img");
    const urls: string[] = [];

    images.forEach((img) => {
        const src = img.getAttribute("src");
        if (src) {
            urls.push(new URL(src, baseURL).toString());
        }
    });

    return urls;
}
interface ExtractedPageData {
    url: string;
    heading: string;
    first_paragraph: string;
    outgoing_links: string[];
    image_urls: string[];
}

function extractPageData(html: string, pageURL: string): ExtractedPageData {
    return {
        url: pageURL,
        heading: getHeadingFromHTML(html),
        first_paragraph: getFirstParagraphFromHTML(html),
        outgoing_links: getURLsFromHTML(html, pageURL),
        image_urls: getImagesFromHTML(html, pageURL),
    };
}




class ConcurrentCrawler {
    baseURL: string;
    pages: Record<string, number> = {};
    limit: ReturnType<typeof pLimit>;

    constructor(baseURL: string, maxConcurrency: number) {
        this.baseURL = baseURL;
        this.pages = {};
        this.limit = pLimit(maxConcurrency);
    }

    addPageVisit(currentURL: string): boolean {
        const normalizedURL = normalizeURL(currentURL);


        const existing = this.pages[normalizedURL];
        if (existing !== undefined) {
            this.pages[normalizedURL] = existing + 1;
            return false;
        }
        this.pages[normalizedURL] = 1;
        return true;
    }

    // Promise based concurrency
    // https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise
    private async getHTML(url: string): Promise<string | null> {
        return await this.limit(async () => {


            try {
                const response = await fetch(url, {
                    headers: {
                        "User-Agent": "BootCrawler/1.0",
                    },
                });

                if (response.status >= 400) {
                    console.error(
                        `HTTP error: ${response.status} ${response.statusText} for ${url}`
                    );
                    return null;
                }

                const contentType = response.headers.get("content-type");
                if (!contentType || !contentType.includes("text/html")) {
                    console.error(`Unexpected content-type: ${contentType} for ${url}`);
                    return null;
                }

                return response.text();
            } catch (error) {
                console.error(`Failed to fetch ${url}: ${error}`);
                return null;
            }
        });
    }

    private async crawlPage(
        baseURL: string,
        currentURL: string,
        pages: Record<string, number> = {},
    ): Promise<Record<string, number>> {


        const baseURLObj = new URL(baseURL);
        const currentURLObj = new URL(currentURL);

        if (currentURLObj.hostname !== baseURLObj.hostname) {
            return pages;
        }

        const normalizedURL = normalizeURL(currentURL);

        const existing = pages[normalizedURL];
        if (existing !== undefined) {
            pages[normalizedURL] = existing + 1;
            return pages;
        }

        pages[normalizedURL] = 1;

        console.log(`Crawling ${currentURL}`);

        const html = await this.getHTML(currentURL);
        if (!html) {
            return pages;
        }

        const urls = getURLsFromHTML(html, currentURL);

        for (const url of urls) {
            pages = await this.crawlPage(baseURL, url, pages);
        }

        return pages;
    }
}

export { normalizeURL, getHeadingFromHTML, getFirstParagraphFromHTML, getURLsFromHTML, getImagesFromHTML, extractPageData, ConcurrentCrawler };