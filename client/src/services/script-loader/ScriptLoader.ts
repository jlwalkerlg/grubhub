class ScriptLoader {
  public load(src: string): Promise<void> {
    return new Promise((resolve) => {
      const script = document.createElement("script");
      script.src = src;
      script.addEventListener("load", () => resolve());
      document.body.appendChild(script);
    });
  }
}

export default new ScriptLoader();
