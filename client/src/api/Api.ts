export default class Api {
  // TODOO: pull api url from config
  private baseUrl: string = "http://localhost:5000";

  protected getUrl(path: string): string {
    if (path.startsWith("/")) {
      path = path.substr(1);
    }

    return `${this.baseUrl}/${path}`;
  }
}
