import { describe, expect, test } from "vitest";
import { normalizeURL, getHeadingFromHTML, getFirstParagraphFromHTML, getImagesFromHTML, getURLsFromHTML, extractPageData } from "./crawl.ts";

describe("normalizeURL", () => {
    test("normalizes https URL with trailing slash", () => {
        expect(normalizeURL("https://www.boot.dev/blog/path/")).toBe(
            "www.boot.dev/blog/path"
        );
    });

    test("normalizes https URL without trailing slash", () => {
        expect(normalizeURL("https://www.boot.dev/blog/path")).toBe(
            "www.boot.dev/blog/path"
        );
    });

    test("normalizes http URL with trailing slash", () => {
        expect(normalizeURL("http://www.boot.dev/blog/path/")).toBe(
            "www.boot.dev/blog/path"
        );
    });

    test("normalizes http URL without trailing slash", () => {
        expect(normalizeURL("http://www.boot.dev/blog/path")).toBe(
            "www.boot.dev/blog/path"
        );
    });
});

describe("getHeadingFromHTML", () => {
    test("getHeadingFromHTML basic", () => {
        const inputBody = `<html><body><h1>Test Title</h1></body></html>`;
        const actual = getHeadingFromHTML(inputBody);
        const expected = "Test Title";
        expect(actual).toEqual(expected);
    });

    test("returns the text content of the <h1> tag if present", () => {
        const html = "<h1>Hello, world!</h1>";
        expect(getHeadingFromHTML(html)).toBe("Hello, world!");
    });

    test("returns the text content of the <h2> tag as a fallback if <h1> is not present", () => {
        const html = "<h2>Hello, world!</h2>";
        expect(getHeadingFromHTML(html)).toBe("Hello, world!");
    });

    test("returns an empty string if neither an <h1> nor an <h2> tag is found", () => {
        const html = "<p>Hello, world!</p>";
        expect(getHeadingFromHTML(html)).toBe("");
    });
});

describe("getFirstParagraphFromHTML", () => {
    test("returns the text content of the first <p> tag", () => {
        const html = "<p>Hello, world!</p>";
        expect(getFirstParagraphFromHTML(html)).toBe("Hello, world!");
    });

    test("returns an empty string if no <p> tag is found", () => {
        const html = "<div>Hello, world!</div>";
        expect(getFirstParagraphFromHTML(html)).toBe("");
    });

    test("getFirstParagraphFromHTML main priority", () => {
        const inputBody = `
    <html><body>
      <p>Outside paragraph.</p>
      <main>
        <p>Main paragraph.</p>
      </main>
    </body></html>
  `;
        const actual = getFirstParagraphFromHTML(inputBody);
        const expected = "Main paragraph.";
        expect(actual).toEqual(expected);
    });

});

describe("getURLsFromHTML", () => {
    //Las URL relativas se convierten en URL absolutas.
    //Encontrarás todas las <a>etiquetas en un cuerpo de HTML.

    test("getURLsFromHTML absolute", () => {
        const inputURL = "https://crawler-test.com";
        const inputBody = `<html><body><a href="/path/one"><span>Boot.dev</span></a></body></html>`;

        const actual = getURLsFromHTML(inputBody, inputURL);
        const expected = ["https://crawler-test.com/path/one"];

        expect(actual).toEqual(expected);
    });
})

describe("getImagesFromHTML", () => {
    //Encontrarás todas las <img>etiquetas en un cuerpo de HTML.
    test("getImagesFromHTML relative", () => {
        const inputURL = "https://crawler-test.com";
        const inputBody = `<html><body><img src="/logo.png" alt="Logo"></body></html>`;

        const actual = getImagesFromHTML(inputBody, inputURL);
        const expected = ["https://crawler-test.com/logo.png"];

        expect(actual).toEqual(expected);
    });
})

describe("extractPageData", () => {
    test("extractPageData basic", () => {
        const inputURL = "https://crawler-test.com";
        const inputBody = `
    <html><body>
      <h1>Test Title</h1>
      <p>This is the first paragraph.</p>
      <a href="/link1">Link 1</a>
      <img src="/image1.jpg" alt="Image 1">
    </body></html>
  `;

        const actual = extractPageData(inputBody, inputURL);
        const expected = {
            url: "https://crawler-test.com",
            heading: "Test Title",
            first_paragraph: "This is the first paragraph.",
            outgoing_links: ["https://crawler-test.com/link1"],
            image_urls: ["https://crawler-test.com/image1.jpg"],
        };

        expect(actual).toEqual(expected);
    });
})