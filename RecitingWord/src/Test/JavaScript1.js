
function() {

	var $translateButton = $("#TranslateButton");
	var $sourceText = $("#srcText");
	var $destText = $("#destText");
	var $progressContainer = $(".progressImgPanel");
	var $clearInputButton = $(".clearInput");
	var $swapButton = $(".swapButton");
	var $errorMessage = $(".errorPanel .error");
	var $speakContainers = $(".speakAndShare");
	var $destinationTextContainer = $(".destinationText");
	var $sourceTextContainer = $(".sourceText");
	var $overlay = $('#Overlay');
	var $definitionsContainer = $('#DefinitionsContainer');
	var translateUrlUrl = constants.siteTranslationUrlPattern;
	var $hipPanel = $(".mt-hip-inline");
	var $hipFrame = $hipPanel.find("#hipFrame");
	var hipUrl = "home/hip";
	var errorMsgGeneric = "We are experiencing some service problems. Refresh page or try again later.";
	var errorMsgUnsupportedLanguage = "Sorry, one of the parameters specified is incorrect.";
	var errorMsgIpThrottled = "<p>We’ve received an unusual volume of traffic from your IP address and will be unable to translate this webpage at the moment.</p><p>Please wait some time before trying again.</p>";
	var errorMsgThrottled = "<p>Oops, looks like our service is down and we are looking into it.</p><p>We will be unable to translate this webpage at the moment.</p><p>Please try again later.</p>";
	var tknRenewalAttempts = 0;
	var self = this; this.events = new EventService; createReadonlyProperty(window, "ALL_LANGUAGES",
		{
			"-": new Language(constants.autoDetectKey, "", "自动检测"), da: new Language("da", "da", "丹麦语", false), uk: new Language("uk", "uk", "乌克兰语", false), ur: new Language("ur", "ur", "乌尔都语", true), ru: new Language("ru", "ru", "俄语", false), bg: new Language("bg", "bg", "保加利亚语", false), tlh: new Language("tlh", "tlh", "克林贡语", false), "tlh-Qaak": new Language("tlh-Qaak", "tlh-Qaak", "克林贡语 (pIqaD)", false), hr: new Language("hr", "hr", "克罗地亚语", false), otq: new Language("otq", "otq", "克雷塔罗奥托米语", false), ca: new Language("ca", "ca", "加泰隆语", false), hu: new Language("hu", "hu", "匈牙利语", false), af: new Language("af", "af", "南非荷兰语", false), hi: new Language("hi", "hi", "印地语", false), id: new Language("id", "id", "印度尼西亚语", false), tr: new Language("tr", "tr", "土耳其语", false), ty: new Language("ty", "ty", "塔希提语", false), "sr-Latn": new Language("sr-Latn", "sr-Latn", "塞尔维亚语 (拉丁文)", false), "sr-Cyrl": new Language("sr-Cyrl", "sr-Cyrl", "塞尔维亚语 (西里尔文)", false), cy: new Language("cy", "cy", "威尔士语", false), bn: new Language("bn", "bn", "孟加拉语", false), yua: new Language("yua", "yua", "尤卡坦玛雅语", false), he: new Language("he", "he", "希伯来语", true), el: new Language("el", "el", "希腊语", false), de: new Language("de", "de", "德语", false), it: new Language("it", "it", "意大利语", false), lv: new Language("lv", "lv", "拉脱维亚语", false), no: new Language("no", "no", "挪威语", false), cs: new Language("cs", "cs", "捷克语", false), fj: new Language("fj", "fj", "斐济", false), sk: new Language("sk", "sk", "斯洛伐克语", false), sl: new Language("sl", "sl", "斯洛文尼亚语", false), sw: new Language("sw", "sw", "斯瓦希里语", false), ja: new Language("ja", "ja", "日语", false), ko: new Language("ko", "ko", "朝鲜语", false), to: new Language("to", "to", "汤加语", false), fr: new Language("fr", "fr", "法语", false), pl: new Language("pl", "pl", "波兰语", false), "bs-Latn": new Language("bs-Latn", "bs-Latn", "波斯尼亚语", false), fa: new Language("fa", "fa", "波斯语", true), th: new Language("th", "th", "泰语", false), ht: new Language("ht", "ht", "海地克里奥尔语", false), et: new Language("et", "et", "爱沙尼亚语", false), sv: new Language("sv", "sv", "瑞典语", false), mww: new Language("mww", "mww", "白苗文", false), lt: new Language("lt", "lt", "立陶宛语", false), "zh-CHS": new Language("zh-CHS", "zh-CHS", "简体中文", false), yue: new Language("yue", "yue", "粤语(繁体)", false), "zh-CHT": new Language("zh-CHT", "zh-CHT", "繁体中文", false), ro: new Language("ro", "ro", "罗马尼亚语", false), fi: new Language("fi", "fi", "芬兰语", false), en: new Language("en", "en", "英语", false), nl: new Language("nl", "nl", "荷兰语", false), fil: new Language("fil", "fil", "菲律宾语", false), sm: new Language("sm", "sm", "萨摩亚语", false), pt: new Language("pt", "pt", "葡萄牙语", false), es: new Language("es", "es", "西班牙语", false), vi: new Language("vi", "vi", "越南语", false), ar: new Language("ar", "ar", "阿拉伯语", true), mg: new Language("mg", "mg", "马尔加什语", false), ms: new Language("ms", "ms", "马来语", false), mt: new Language("mt", "mt", "马耳他语", false)
		});
	var attributionData =
		{
			mww:
			{
				href: "https://www.microsoft.com/translator/community.aspx#Hmong", text: "与苗语合作伙伴合作"
			}, ur:
			{
				href: "https://www.microsoft.com/translator/community.aspx#JNU", text: "与JNU合作"
			}, "tlh-Qaak":
			{
				href: "https://www.microsoft.com/translator/community.aspx#Klingon", text: "与CBS、Paramount 和 KLI合作"
			}, tlh:
			{
				href: "https://www.microsoft.com/translator/community.aspx#Klingon", text: "与CBS、Paramount 和 KLI合作"
			}, lv:
			{
				href: "https://www.microsoft.com/translator/community.aspx#Tilde", text: "与Tilde合作"
			}, yua:
			{
				href: "https://www.microsoft.com/translator/community.aspx#Maya", text: "与金塔纳罗奥玛雅跨文化交际大学合作"
			}, otq:
			{
				href: "https://www.microsoft.com/translator/community.aspx#Otomi", text: "与克雷塔罗州政府合作"
			}, cy:
			{
				href: "https://www.microsoft.com/translator/community.aspx#NationalAssembly", text: "与威尔士国民议会合作"
			}, sw:
			{
				href: "https://www.microsoft.com/translator/community.aspx#Kiswahili", text: "与Translators without Borders合作"
			}, "sw-TZ":
			{
				href: "https://www.microsoft.com/translator/community.aspx#Kiswahili", text: "与Translators without Borders合作"
			}
		};
	var translator = new Translator(
		{
			translationUrl: constants.translateUrl, src: $sourceText, onTranslateError:
			function () {
				$progressContainer.css("visibility", "hidden")
			}
		}); if (constants.dictionary.isDictionaryEnabled) {
			self.minSizeBeforeReflow = 500; self.textAreaSmallWidth = 300; self.containerWidth = "50%"; self.translateButtonWidth = 130; self.panelMaxWidth = 1158; self.marginOfSpacer = 20; self.spacerWidth = self.translateButtonWidth + self.marginOfSpacer; self.marginAroundMobileTextAreaWidth = 20; $(window).resize(
				function () {

					var panelWidth = $(".panel").width();
					var calc = (panelWidth - self.spacerWidth) / 2; if (panelWidth <= self.minSizeBeforeReflow) {
						if (panelWidth < self.minSizeBeforeReflow) {
							$(".translationContainer").css("width", (panelWidth < self.textAreaSmallWidth ? self.textAreaSmallWidth : panelWidth - self.marginAroundMobileTextAreaWidth) + "px"); $(".mt-alternate-translations-container").css("width", (panelWidth < self.textAreaSmallWidth ? self.textAreaSmallWidth : panelWidth - self.marginAroundMobileTextAreaWidth) + "px")
						} else {
							$(".translationContainer").css("width", self.minSizeBeforeReflow + "px"); $(".mt-alternate-translations-container").css("width", self.minSizeBeforeReflow + "px")
						}
					}
					else {
						if (panelWidth > self.panelMaxWidth) {
							$(".translationContainer").css("width", self.minSizeBeforeReflow + "px"); $(".mt-alternate-translations-container").css("width", self.minSizeBeforeReflow + "px")
						} else {
							$(".translationContainer").css("width", Math.max(calc, self.textAreaSmallWidth) + "px"); $(".mt-alternate-translations-container").css("width", Math.max(calc, self.textAreaSmallWidth) + "px")
						}
					}
				});
			var dictionaryView = new DictionaryView;
			var clearDictionary =
				function () {
					dictionaryView.clear()
				};
			var lookupDictionaryEntryForText =
				function (text) {
					if (text.length <= constants.dictionary.numberOfTextCharacters) {
						dictionaryView.lookup(srcLanguageDD.getSelectedLanguage().lang, srcLanguageDD.getSelectedLanguage().display, destLanguageDD.getSelectedLanguage().lang, destLanguageDD.getSelectedLanguage().display, text)
					}
				}; dictionaryView.events.addListener("onAfterLayout",
					function (evt) {
						if (self.containerLayoutWidth != null && !isNaN(self.containerLayoutWidth)) {
							$(".translationContainer").css("width", self.containerLayoutWidth + "px"); $(".mt-alternate-translations-container").css("width", self.containerLayoutWidth + "px")
						}
					}); dictionaryView.events.addListener("onBeforeLayout",
						function (evt)
						{
							self.containerLayoutWidth = $($(".translationContainer").eq(1)).width(); $(".translationContainer").css("width", self.containerWidth); $(".translationContainer").css("max-width", self.minSizeBeforeReflow + "px"); $(".mt-alternate-translations-container").css("width", self.containerWidth); $(".mt-alternate-translations-container").css("max-width", self.minSizeBeforeReflow + "px")
						}); dictionaryView.events.addListener("onExamples",
							function (evt)
							{
								helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.examples)
							}); dictionaryView.events.addListener("onDisclaimerClicked",
								function (evt) {
									helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.disclaimer)
								}); dictionaryView.events.addListener("onLookup",
									function (evt) {
										helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.lookup)
									}); dictionaryView.events.addListener("onNextExampleClicked",
										function (evt) {
											helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.next)
										}); dictionaryView.events.addListener("onPreviousExampleClicked",
											function (evt) {
												helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.previous)
											}); dictionaryView.events.addListener("onShowLess",
												function (evt) {
													helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.showLess)
												}); dictionaryView.events.addListener("onShowMore",
													function (evt) {
														helpers.telemetryServices.dictionaryAction.logEvent(constants.telemetry.EventPropertyValueBag.dictionary.showMore)
													}); self.events.addListener("onClearInputButtonClickOrEnter",
														function (evt) {
															clearDictionary()
														}); self.events.addListener("onLanguageChanged",
															function (evt) {
																clearDictionary()
															}); self.events.addListener("onHighlighterOnHighlighted",
																function (evt) {
																	lookupDictionaryEntryForText(evt.data.text)
																}); self.events.addListener("onSourceTextTextChanged",
																	function (evt) {

																		var inputText = $sourceText.val() || ""; if (inputText.length == 0) {
																			clearDictionary()
																		}
																	}); translator.events.addListener("endTranslating",
																		function (evt) {
																			try {

																				var paragraphs = evt.data.paragraphs; if (paragraphs && paragraphs.length == 1) {
																					lookupDictionaryEntryForText(paragraphs[0].text)
																				}
																			} catch (ex) {
																			}
																		})
		}
	var flyoutPosition = $('html').hasClass('rtl') === true ? "left" : "right";
	var srcFlyout = new Flyout(
		{
			parent: $('.sourceText div.mobile.menu'), flyout: $('.sourceText div.mobile.voiceOptions'), show: flyoutPosition
		});
	var dstFlyout = new Flyout(
		{
			parent: $('.destinationText div.mobile.menu'), flyout: $('.destinationText div.mobile.voiceOptions'), show: flyoutPosition
		});
	var hiddenClonedSourceText = $('<div />').appendTo($sourceText.parent()).addClass("clonedTextarea").css(
		{
			display: "none", "white-space": "pre-wrap", "word-wrap": "break-word", "overflow-wrap": "break-word"
		});
	var resizeSourceTextArea =
		function () {

			var content = $sourceText.val(); content = hiddenClonedSourceText.text(content).html(); content = content.replace(/\n/g, '<br>'); hiddenClonedSourceText.html(content + '<br class="lbr"><br />'); $(this).css('height', hiddenClonedSourceText.innerHeight())
		}; $sourceText.bind('input propertychange', resizeSourceTextArea);
	var MT_ClassNames =
		{
			ATTRIBUTION: ".attribution", SOURCE_TEXT: ".sourceText", DESTINATION_TEXT: ".destinationText"
		};
	var attributionService =
		function (sel, language) {

			var attribution = attributionData[language];
			var $attribution = $(sel + " > " + MT_ClassNames.ATTRIBUTION); if (attribution) {
				$attribution.html("<a href=\"" + attribution.href + "\" target=\"_blank\">" + attribution.text + "</a>"); $attribution.show()
			} else {
				$attribution.text(''); $attribution.hide()
			}
		};
	var determineInitialDefaultLang =
		function () {

			var result = $.cookie("srcLang") || constants.autoDetectKey; if ("" === "" && jQuery.inArray("", constants.referrers) >= 0) {
				result = constants.autoDetectKey
			} return result
		};
	var srcLanguageDD = new LanguageDropdown(
		{
			src: $(".sourceText"), lang: "", defaultLang: determineInitialDefaultLang(), autoDetectFormatString: "
{
			0}（已自动检测）",onLanguageChanged:
function(event) {
			if (!event.data.to) {
				return
			} self.events.fire("onLanguageChanged");
			var lang = event.data.to; translator.sourceLocale = lang; translator.retranslate(); $.cookie("srcLang", lang.id,
				{
					expires: 365
				});
			var textDirection = lang.getTextDirection(); $overlay.css("direction", textDirection); $definitionsContainer.css("direction", textDirection); if (asString($sourceText.val(), "").length > 0) {
				$sourceText.css("direction", textDirection)
			} attributionService(MT_ClassNames.SOURCE_TEXT, lang.lang); if (asString(lang.lang, "").toUpperCase() === "TLH-QAAK") {
				$sourceText.addClass("klingon")
			} else {
				$sourceText.removeClass("klingon")
			} if (asString(lang.lang, "").length > 0) {
				$swapButton.addClass("active")
			} else {
				$swapButton.removeClass("active")
			}
		}});
var destLanguageDD = new LanguageDropdown(
	{
		src: $destinationTextContainer, lang: "", defaultLang: $.cookie("destLang") || "zh", onLanguageChanged:
		function (event) {
			if (!event.data.to) {
				return
			} self.events.fire("onLanguageChanged");
			var lang = event.data.to; translator.destinationLocale = lang; translator.retranslate(); $.cookie("destLang", lang.id,
				{
					expires: 365
				}); $destText.css("direction", lang.getTextDirection()); attributionService(MT_ClassNames.DESTINATION_TEXT, lang.lang); if (asString(lang.lang, "").toUpperCase() === "TLH-QAAK") {
					$destText.addClass("klingon")
				} else {
				$destText.removeClass("klingon")
			}
		}
	});
var clickableTranslateButton = new HoverButton(
	{
		src: $translateButton, inClass: "Over", outClass: "Out"
	});

$translateButton.clickOrEnter(
		function ()
		{

			var srcText = $sourceText.val().trim(); self.events.fire("OnTranslateButtonOnClickOrEnter",
				{
					length: srcText.length, isUrl: URL_ONLY_REGEX.test(srcText)
				});
			if (URL_ONLY_REGEX.test(srcText))
			{
				var tab = window.open(getUrlTranslationUrl(srcText), '_blank');
				if (tab)
				{
					tab.focus()
				}
				else
				{
					window.location.href = getUrlTranslationUrl(srcText)
				}
			}
			else
			{
				translator.retranslate()
			}
		});
var clickableLanguageListItems = new HoverButton(
	{
		src: $(".LS_Item"), inClass: "hovered"
	}); $swapButton.clickOrEnter(
		function () {

			var sourceLang = srcLanguageDD.getSelectedLanguage(); if (sourceLang.isAutoDetect() && asString(sourceLang.lang, "").length === 0) {
				return
			}
			var newSourceText = $destText.text(); $sourceText.val(""); self.events.fire("OnSwapButtonOnClickOrEnter",
				{
					length: newSourceText.trim().length, sourceLang: destLanguageDD.getSelectedLanguage().lang, destinationLang: sourceLang.lang
				}); srcLanguageDD.setSelectedLanguage(destLanguageDD.getSelectedLanguage().lang, null, $swapButton); destLanguageDD.setSelectedLanguage(sourceLang.lang, null, $swapButton); $sourceText.val(newSourceText); $sourceText.css("direction", srcLanguageDD.getSelectedLanguage().getTextDirection()); translator.retranslate()
		}); $clearInputButton.clickOrEnter(
			function () {
				self.events.fire("onClearInputButtonClickOrEnter"); $sourceText.val(""); translator.clearTranslation(); sourceCounter.updateCounter(); $sourceText.focus(); resizeSourceTextArea()
			});
var $feedbackGoodButton = $(".button.rate.up");
var $feedbackBadButton = $(".button.rate.down");
var $ratingThankYou = $(".ratingThankYou");
function addFeedback(iRating) {
	$feedbackGoodButton.hide(); $feedbackBadButton.hide(); $ratingThankYou.show(); try {
		helpers.feedback.logEvent(
			{
				url: constants.ratingApiUrl, data: JSON.stringify(
					{
						sourceText: $sourceText.val(), translatedText: $destText.text(), fromLocale: srcLanguageDD.getSelectedLanguage().lang, toLocale: destLanguageDD.getSelectedLanguage().lang, rating: iRating
					})
			})
	} catch (e) {
	}
}; translator.sourceLocale = srcLanguageDD.getSelectedLanguage(); translator.destinationLocale = destLanguageDD.getSelectedLanguage(); translator.events.addListener("beginTranslating",
	function () {
		$progressContainer.css("visibility", "visible")
	}); translator.events.addListener("textCleared",
		function () {
			$clearInputButton.hide(); $speakContainers.hide()
		}); translator.events.addListener("translationError",
			function (event) {
				$progressContainer.css("visibility", "hidden");
				var errorMsgToShow = errorMsgGeneric;
				var errorResult = event && event.data && event.data.error && event.data.error.res;
				var serviceStatusCode = errorResult && errorResult.status || -1;
				var from = event && event.data && event.data.from; switch (serviceStatusCode) {
					case 400: if (from && from.isAutoDetect()) {
						$destText.text(""); errorMsgToShow = errorMsgUnsupportedLanguage
					} break; case 403: if (++tknRenewalAttempts <= 3) {
						errorMsgToShow = ""; $progressContainer.css("visibility", "visible"); $.ajax("./",
							{
								cache: false, dataType: "html", error:
								function (jqXHR, status, error) {
									showError(errorMsgGeneric); $progressContainer.css("visibility", "hidden")
								}, success:
								function (data, status, jqXHR) {
									translator.retranslate()
								}
							})
					} break; case 429: if ($(document.documentElement).hasClass("lte-ie8")) {
						errorMsgToShow = errorMsgIpThrottled
					} else {
						errorMsgToShow = ""; showHip()
					} break; case 503:
						var serviceErrorMsg = errorResult.statusText && errorResult.statusText.toLowerCase(); if (serviceErrorMsg.indexOf("app throttled") >= 0) {
							errorMsgToShow = errorMsgThrottled; telemetryServices.appIdOverQuota()
						} else if (serviceErrorMsg.indexOf("ip throttled") >= 0) {
							errorMsgToShow = errorMsgIpThrottled; telemetryServices.ipOverQuota()
						} break
				}if (errorMsgToShow) {
					showError(errorMsgToShow)
				}
			}); translator.events.addListener("endTranslating",
				function (event) {
					$progressContainer.css("visibility", "hidden"); hideError();
					var newText = "";
					var isTextExistInSource = false; if (event.data.paragraphs != null) {
						for (
							var i = 0; i < event.data.paragraphs.length; i++) {
							if (event.data.paragraphs[i] && event.data.paragraphs[i].translatedText) {
								isTextExistInSource = isTextExistInSource || event.data.paragraphs[i].translatedText != ""; newText += (event.data.paragraphs[i].translatedText + "\n")
							}
						}
					} $destText.text(newText); if ($destText.prop("scrollHeight") > $destText.height()) {
						$destText.scrollTop($destText.prop("scrollHeight"))
					} if (newText.length > 0) {
						$clearInputButton.show()
					} isTextExistInSource ? $speakContainers.show() : $speakContainers.hide();
					var fromLang = event.data.from; if (fromLang.isAutoDetect()) {

						var language = srcLanguageDD.getLanguageById(fromLang.lang); if (language != null) {
							srcLanguageDD.setSelectedLanguage(constants.autoDetectKey, language, translator)
						}
					}
				});
function getUrlTranslationUrl(url)
{
	if (translateUrlUrl === "")
	{
		return ""
	}
	var destinationUrl = /^https/i.test(url) ? translateUrlUrl.replace(/^http:/, "https:") : translateUrlUrl;
	return destinationUrl.replace("{			url
		}",encodeURIComponent(url.trim())).replace("
{
			from
		}","").replace("
{
			to
		}",encodeURIComponent(destLanguageDD.getSelectedLanguage().lang))}var highlighter = new HighlightingManager;
	var lastWordDefined = null; highlighter.initialize($overlay, $("#destText"), $("#srcText"), "highlighted", destLanguageDD); highlighter.removeListener("onHighlighted"); highlighter.addListener("onHighlighted",
		function (event) {
			if (event.text != lastWordDefined) {
				lastWordDefined = event.text; self.events.fire('onHighlighterOnHighlighted',
					{
						text: event.text, length: event.text !== null ? event.text.trim().length : 0, isSource: event.isSource
					})
			}
		});
	var lastParsedTranslationResults = null; translator.events.addListener("endTranslating",
		function (event) {
			if (event.data != null && event.data.paragraphs != null) {

				var translationResults = highlighter.TranslationResultParser.parseResultsArray($sourceText.val().split("\n"), translator.getTranslationData(), destLanguageDD.getSelectedLanguage().lang); if (translationResults != lastParsedTranslationResults) {
					lastParsedTranslationResults = translationResults; highlighter.setHighlightStatus(false); highlighter.clear(); highlighter.load(translationResults); highlighter.setHighlightStatus(true)
				}
			}
		});
	var sourceMRU = new MRUList(
		{
			container: $("div.translationContainer.sourceText div.languageSelection"), prefix: "smru"
		}); sourceMRU.setCurrentLang(srcLanguageDD.getSelectedLanguage()); sourceMRU.events.addListener("onMruSelected",
			function (event) {
				srcLanguageDD.setSelectedLanguage(event.data.langId, null, sourceMRU)
			}); srcLanguageDD.events.addListener("onLanguageChanged",
				function (event) {
					sourceMRU.setCurrentLang(event.data.to)
				});
	var destinationMRU = new MRUList(
		{
			container: $("div.translationContainer.destinationText div.languageSelection"), prefix: "dmru"
		}); destinationMRU.setCurrentLang(destLanguageDD.getSelectedLanguage()); destinationMRU.events.addListener("onMruSelected",
			function (event) {
				destLanguageDD.setSelectedLanguage(event.data.langId, null, destinationMRU)
			}); destLanguageDD.events.addListener("onLanguageChanged",
				function (event) {
					destinationMRU.setCurrentLang(event.data.to)
				});
	var tts = new Microsoft.JS.TTS(
		{
			ttsServiceUrl: constants.speakServiceUrl, maxCharacters: constants.maxCharsForSpeak
		}); tts.events.addListener("onPlay",
			function () {
				hideError()
			}); tts.events.addListener("onError",
				function () {
					showError(errorMsgGeneric)
				});
	var sourceSpeak = new SpeakAndShare(
		{
			src: $sourceTextContainer.find(".speakAndShare"), tts: tts, searchUrlFormat: constants.searchUrlFormat, textSource: $sourceText, languageDD: srcLanguageDD, languageSpeakDialectsDataUrl: constants.getSpeakLanguagesForLocaleUrl, cookiePrefix: "source"
		});
	var destSpeakAndShare = new SpeakAndShare(
		{
			src: $destinationTextContainer.find(".speakAndShare"), tts: tts, searchUrlFormat: constants.searchUrlFormat, textSource: $destText, languageDD: destLanguageDD, languageSpeakDialectsDataUrl: constants.getSpeakLanguagesForLocaleUrl, cookiePrefix: "dest"
		});
	var sourceCounter = new CharacterCounter(
		{
			src: $sourceText, output: $(".counter")
		}); sourceCounter.updateCounter(); $(document).click(
			function () {
				srcLanguageDD.close(); destLanguageDD.close()
			});
	var inputTextCrc = ""; $sourceText.bind("input.textChanged propertychange.textChanged",
		function (event) {
			self.events.fire("onSourceTextTextChanged");
			var inputText = $sourceText.val() || "";
			var textDirection = ""; if (inputText.length > 0) {
				textDirection = srcLanguageDD.getSelectedLanguage().getTextDirection()
			} $sourceText.css("direction", textDirection);
			var newCrc = inputText.hashCode(); if (newCrc !== inputTextCrc) {
				inputTextCrc = newCrc; highlighter.setHighlightStatus(false); $feedbackGoodButton.show(); $feedbackBadButton.show(); $ratingThankYou.hide()
			}
		}); self.events.addListener("onClearInputButtonClickOrEnter",
			function (evt) {
				helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.clear, 0)
			}); self.events.addListener("OnTranslateButtonOnClickOrEnter",
				function (evt) {
					helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.user, evt.data.length); if (evt.data.isUrl) {
						helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.url, evt.data.length)
					}
				}); self.events.addListener("onHighlighterOnHighlighted",
					function (evt) {
						helpers.telemetryServices.hoverAction.logEvent(evt.data.isSource === true ? constants.telemetry.EventPropertyValueBag.source : constants.telemetry.EventPropertyValueBag.destination)
					}); self.events.addListener("OnSwapButtonOnClickOrEnter",
						function (evt) {
							helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.swap, evt.data.length); helpers.telemetryServices.languageAction.logEvent(constants.telemetry.EventPropertyValueBag.source, constants.telemetry.EventPropertyValueBag.application, evt.data.sourceLang); helpers.telemetryServices.languageAction.logEvent(constants.telemetry.EventPropertyValueBag.destination, constants.telemetry.EventPropertyValueBag.application, evt.data.destinationLang)
						}); translator.events.addListener("onRetranslate",
							function (evt) {
								helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.retranslate, evt.data.length)
							}); srcFlyout.events.addListener("onShow",
								function (event) {
									helpers.telemetryServices.mobileMenuAction.logEvent(constants.telemetry.EventPropertyValueBag.open, constants.telemetry.EventPropertyValueBag.source)
								}); dstFlyout.events.addListener("onShow",
									function (event) {
										helpers.telemetryServices.mobileMenuAction.logEvent(constants.telemetry.EventPropertyValueBag.open, constants.telemetry.EventPropertyValueBag.destination)
									}); $feedbackGoodButton.clickOrEnter(
										function () {
											helpers.telemetryServices.feedbackAction.logEvent(constants.telemetry.EventPropertyValueBag.up); addFeedback(1)
										}); $feedbackBadButton.clickOrEnter(
											function () {
												helpers.telemetryServices.feedbackAction.logEvent(constants.telemetry.EventPropertyValueBag.down); addFeedback(-1)
											}); translator.events.addListener("onTextPasted",
												function (event) {
													helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.paste, event.data.length)
												}); sourceMRU.events.addListener("onMruSelected",
													function (event) {
														helpers.telemetryServices.languageAction.logEvent(constants.telemetry.EventPropertyValueBag.source, constants.telemetry.EventPropertyValueBag.mru, event && event.data ? event.data.langId : "null")
													}); srcLanguageDD.events.addListener("onLanguageChanged",
														function (event) {
															if (event && event.data && event.data.origin === null) {
																helpers.telemetryServices.languageAction.logEvent(constants.telemetry.EventPropertyValueBag.source, constants.telemetry.EventPropertyValueBag.dropdown, event && event.data && event.data.to ? event.data.to.lang : "null")
															}
														}); destinationMRU.events.addListener("onMruSelected",
															function (event) {
																helpers.telemetryServices.languageAction.logEvent(constants.telemetry.EventPropertyValueBag.destination, constants.telemetry.EventPropertyValueBag.mru, event && event.data ? event.data.langId : "null")
															}); destLanguageDD.events.addListener("onLanguageChanged",
																function (event) {
																	if (event && event.data && event.data.origin === null) {
																		helpers.telemetryServices.languageAction.logEvent(constants.telemetry.EventPropertyValueBag.destination, constants.telemetry.EventPropertyValueBag.dropdown, event && event.data && event.data.to ? event.data.to.lang : "null")
																	}
																}); sourceSpeak.events.addListener("onSpeak",
																	function (event) {
																		helpers.telemetryServices.ttsAction.logEvent(constants.telemetry.EventPropertyValueBag.speak, constants.telemetry.EventPropertyValueBag.source, event.data.isMobile, event.data.gender, event.data.locale)
																	}); sourceSpeak.events.addListener("onDialectChanged",
																		function (event) {
																			helpers.telemetryServices.ttsAction.logEvent(constants.telemetry.EventPropertyValueBag.dialectChanged, constants.telemetry.EventPropertyValueBag.source, event.data.isMobile, event.data.gender, event.data.locale)
																		}); sourceSpeak.events.addListener("onGenderChanged",
																			function (event) {
																				helpers.telemetryServices.ttsAction.logEvent(constants.telemetry.EventPropertyValueBag.genderChanged, constants.telemetry.EventPropertyValueBag.source, event.data.isMobile, event.data.gender, event.data.locale)
																			}); destSpeakAndShare.events.addListener("onSpeak",
																				function (event) {
																					helpers.telemetryServices.ttsAction.logEvent(constants.telemetry.EventPropertyValueBag.speak, constants.telemetry.EventPropertyValueBag.destination, event.data.isMobile, event.data.gender, event.data.locale)
																				}); destSpeakAndShare.events.addListener("onShare",
																					function () {
																						helpers.telemetryServices.shareAction.logEvent(constants.telemetry.EventPropertyValueBag.open, constants.telemetry.EventPropertyValueBag.share)
																					}); destSpeakAndShare.events.addListener("onShareSelected",
																						function (event) {

																							var share = constants.telemetry.share.unknown; if (event.data.isEmail) {
																								share = constants.telemetry.share.email
																							} else if (event.data.isFacebook) {
																								share = constants.telemetry.share.facebook
																							} else if (event.data.isLinkedIn) {
																								share = constants.telemetry.share.linkedin
																							} else if (event.data.isTwitter) {
																								share = constants.telemetry.share.twitter
																							} helpers.telemetryServices.shareAction.logEvent(constants.telemetry.EventPropertyValueBag.share, share)
																						}); destSpeakAndShare.events.addListener("onSearch",
																							function (event) {
																								helpers.telemetryServices.searchAction.logEvent(constants.telemetry.EventPropertyValueBag.open, event.data.isMobile)
																							}); destSpeakAndShare.events.addListener("onDialectChanged",
																								function (event) {
																									helpers.telemetryServices.ttsAction.logEvent(constants.telemetry.EventPropertyValueBag.dialectChanged, constants.telemetry.EventPropertyValueBag.destination, event.data.isMobile, event.data.gender, event.data.locale)
																								}); destSpeakAndShare.events.addListener("onGenderChanged",
																									function (event) {
																										helpers.telemetryServices.ttsAction.logEvent(constants.telemetry.EventPropertyValueBag.genderChanged, constants.telemetry.EventPropertyValueBag.destination, event.data.isMobile, event.data.gender, event.data.locale)
																									}); $sourceText.focusout(
																										function (evt) {
																											helpers.telemetryServices.translateAction.logEvent(constants.telemetry.EventPropertyValueBag.lostFocus, $sourceText.val().trim().length)
																										}); telemetryServices.pageView(constants.telemetry.EventPropertyValueBag.home);
	function getInstallTelemetryTarget(element) {
		if (!element || typeof (element.hasClass) !== "
function")return constants.telemetry.EventPropertyValueBag.install.unknown;else if(element.hasClass("mt- iphone"))return constants.telemetry.EventPropertyValueBag.install.iPhone;else if(element.hasClass("mt- android"))return constants.telemetry.EventPropertyValueBag.install.android;else if(element.hasClass("mt- windows - phone"))return constants.telemetry.EventPropertyValueBag.install.windows;else if(element.hasClass("mt- edge - extension"))return constants.telemetry.EventPropertyValueBag.install.edge;else return constants.telemetry.EventPropertyValueBag.install.unknown}
		var $bannerInstallAppButton = $(".mt-banner-install-btn"); $bannerInstallAppButton.clickOrEnter(
			function () {

				var target = getInstallTelemetryTarget($bannerInstallAppButton); helpers.telemetryServices.appsAction.logEvent(constants.telemetry.EventPropertyValueBag.install.banner, target)
			});
		var $footerInstallAppButton = $(".mt-footer-install-btn"); $footerInstallAppButton.clickOrEnter(
			function () {

				var target = getInstallTelemetryTarget($(this)); helpers.telemetryServices.appsAction.logEvent(constants.telemetry.EventPropertyValueBag.install.links, target)
			});
		var initialSourceLanguage = srcLanguageDD.getSelectedLanguage();
		var initialDestLanguage = destLanguageDD.getSelectedLanguage(); sourceSpeak.setLocale(initialSourceLanguage.lang); destSpeakAndShare.setLocale(initialDestLanguage.lang); if ($sourceText.length > 0 && $sourceText.val().length > 0) {

			var endOfText = $sourceText.val().length; $sourceText.focus();
			var input = $sourceText[0]; if (input.setSelectionRange) {
				input.setSelectionRange(endOfText, endOfText)
			} else if (input.createTextRange) {

				var range = input.createTextRange(); range.collapse(true); range.moveEnd('character', endOfText); range.moveStart('character', endOfText); range.select()
			}
		}
		var hipAttempts = 0;
		function hideHip(success) {
			if (showHipTimer) {
				clearTimeout(showHipTimer); showHipTimer = 0
			} $hipFrame.attr("src", ""); $hipPanel.hide(); if (success === true) {
				translator.retranslate(); hipAttempts++
			} telemetryServices.captchaDone(success, hipAttempts)
		}
		var showHipTimer;
		function showHip() {
			if (showHipTimer) {
				clearTimeout(showHipTimer); showHipTimer = 0
			} hipAttempts = 0; if (!$hipFrame.attr("src")) {
				$hipFrame.attr("src", hipUrl)
			} showHipTimer = setTimeout(
				function () {
					hideError(); $hipPanel.show(); $(window).scrollTop(0); $(window).scrollLeft(0); telemetryServices.captchaDisplayed()
				}, 1500)
		} $(window).on("message",
			function (evt) {

				var origin = evt.origin || evt.originalEvent.origin;
				var data = evt.data || evt.originalEvent.data; if (typeof (data) === "string") {
					data = JSON.parse(data)
				} if (origin != window.location.origin || !data) {
					return
				} switch (data.message) {
					case "HipDone": hideHip(data.success); break; case "HipAttemptFailed": hipAttempts++; break
				}
			});
		function showError(message) {
			if (typeof (message) !== "string") {
				message = errorMsgGeneric
			} $errorMessage.html(message); $errorMessage.show()
		};
		function hideError() {
			$errorMessage.hide()
		};
	}
