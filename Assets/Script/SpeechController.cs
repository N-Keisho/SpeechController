using System.Collections;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class SpeechController : MonoBehaviour
{
    public List<string> words = new List<string>();
    public Dictionary<string, bool> flags = new Dictionary<string, bool>();
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    // Start is called before the first frame update
    void Start()
    {

        foreach (string word in words)
        {
            flags.Add(word, false);
            keywords.Add(word, () =>
            {
                Debug.Log("Keyword: " + word);
                try
                {
                    flags[word] = true;
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
            });
        };

        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}
