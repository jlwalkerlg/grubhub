export const loadScript = (src: string): Promise<void> => {
  return new Promise((resolve) => {
    if (document.querySelector(`script[src="${src}"]`) !== null) {
      resolve();
      return;
    }

    const script = document.createElement("script");
    script.src = src;
    script.addEventListener("load", () => resolve());
    document.body.appendChild(script);
  });
};

export async function sleep(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}
