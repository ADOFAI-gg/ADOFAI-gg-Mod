﻿using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using ADOFAI_GG.API.TinyJSON.Types;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.API.Types {
    public class Ranking : Person {
        public readonly PlayLog BestPlay;

        internal Ranking(int id, string name, double totalBpm, double totalPp, PlayLog bestPlay) : base(id, name,
            totalBpm, totalPp) {
            BestPlay = bestPlay;
        }

        public static IEnumerator FromJson(Variant obj, Func<Ranking, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(PlayLog bestPlay) {
                yield return callback(new Ranking(
                    obj.GetOrNull("id").ToInt32(CultureInfo.InvariantCulture),
                    obj.GetOrNull("name")?.ToString(CultureInfo.InvariantCulture),
                    obj.GetOrNull("totalBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                    obj.GetOrNull("totalPp")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                    bestPlay
                ));
            }
            yield return RequestCo.RequestPlayLog(obj["bestPlay"]["id"].ToInt32(CultureInfo.InvariantCulture), Callback, fail);
        }
        
        
        public new static async Task<Ranking> FromJson(Variant obj) {
            var bestPlay = await Request.RequestPlayLog(obj["bestPlay"]["id"].ToInt32(CultureInfo.InvariantCulture));

            return new Ranking(
                obj.GetOrNull("id").ToInt32(CultureInfo.InvariantCulture),
                obj.GetOrNull("name")?.ToString(CultureInfo.InvariantCulture),
                obj.GetOrNull("totalBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("totalPp")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                bestPlay
            );
        }
    }
}