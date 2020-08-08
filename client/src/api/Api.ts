export default class Api {
  private baseUrl: string = "http://localhost:8080";

  protected getUrl(path: string): string {
    const url = `${this.baseUrl}/${path}`;

    if (url.endsWith("/")) {
      return url.substr(0, url.length - 1);
    }

    return url;
  }
}
