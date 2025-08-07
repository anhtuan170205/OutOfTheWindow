using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TypingTextEffect : MonoBehaviour
{
    [Header("Typing Settings")]
    [SerializeField] private float delayBetweenChars = 0.05f;
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource typingAudio;

    private TextMeshProUGUI textComponent;
    private string originalText;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        originalText = textComponent.text;
        textComponent.text = "";
    }

    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        textComponent.text = originalText;
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            var vertexColors = textInfo.meshInfo[matIndex].colors32;
            for (int j = 0; j < 4; j++)
                vertexColors[vertexIndex + j].a = 0;
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        if (typingAudio != null && !typingAudio.isPlaying)
            typingAudio.Play();

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            var vertexColors = textInfo.meshInfo[matIndex].colors32;

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                byte alpha = (byte)Mathf.Lerp(0, 255, elapsed / fadeDuration);
                for (int j = 0; j < 4; j++)
                    vertexColors[vertexIndex + j].a = alpha;

                textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                elapsed += Time.deltaTime;
                yield return null;
            }

            for (int j = 0; j < 4; j++)
                vertexColors[vertexIndex + j].a = 255;

            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            yield return new WaitForSeconds(delayBetweenChars);
        }

        if (typingAudio != null && typingAudio.isPlaying)
            typingAudio.Stop();
    }
}
