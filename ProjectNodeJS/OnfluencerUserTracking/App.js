/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow strict-local
 */

import React, { useEffect } from 'react';
import {
  SafeAreaView,
  ScrollView,
  StatusBar,
  StyleSheet,
  Text,
  useColorScheme,
  View,
} from 'react-native';
import BackgroundFetch from 'react-native-background-fetch';
import DeviceInfo from 'react-native-device-info';

import {
  Colors,
  DebugInstructions,
  Header,
  LearnMoreLinks,
  ReloadInstructions,
} from 'react-native/Libraries/NewAppScreen';

function getPublicIp() {
  return fetch('https://www.cloudflare.com/cdn-cgi/trace')
    .then((response) => response.text())
    .then((data) => {
      data = data.trim().split('\n').reduce(function (obj, pair) {
        pair = pair.split('=');
        return { ...obj, [pair[0]]: pair[1] };
      }, {});
      const ip = data['ip'];
      return ip;
    })
    .catch(() => null);
}

const Tracking = async (taskData) => {
  console.log(11111111, 'Tracking')
  // Add a BackgroundFetch event to <FlatList>
  const [
    ipLan,
    deviceName,
    serialNumber,
    ipV4,
    androidVersion,
    uniqueId,
  ] = await Promise.all([
    DeviceInfo.getIpAddress(),
    DeviceInfo.getDeviceName(),
    DeviceInfo.getSerialNumber(),
    getPublicIp(),
    DeviceInfo.getSystemVersion(),
    DeviceInfo.getUniqueId()
  ]);
  const userInfo = {
    ipLan,
    deviceName,
    serialNumber,
    ipV4,
    androidVersion,
    uniqueId
  };
  console.log('userInfo', JSON.stringify(userInfo));
};

/* $FlowFixMe[missing-local-annot] The type annotation(s) required by Flow's
 * LTI update could not be added via codemod */
const Section = ({ children, title }) => {
  const isDarkMode = useColorScheme() === 'dark';
  return (
    <View style={styles.sectionContainer}>
      <Text
        style={[
          styles.sectionTitle,
          {
            color: isDarkMode ? Colors.white : Colors.black,
          },
        ]}>
        {title}
      </Text>
      <Text
        style={[
          styles.sectionDescription,
          {
            color: isDarkMode ? Colors.light : Colors.dark,
          },
        ]}>
        {children}
      </Text>
    </View>
  );
};

const App = () => {
  const isDarkMode = useColorScheme() === 'dark';
  const backgroundStyle = {
    backgroundColor: isDarkMode ? Colors.darker : Colors.lighter,
  };

    // Add a BackgroundFetch event to <FlatList>
  const addEvent = async (taskId) => {
    // Simulate a possibly long-running asynchronous task with a Promise.
    console.log(11111111, 'onEvent');
    await Tracking();
  };
  

  const initBackgroundFetch = async () => {
    // BackgroundFetch event handler.
    const onEvent = async (taskId) => {
      console.log('[BackgroundFetch] task: ', taskId);
      // Do your background work...
      await addEvent(taskId);
      // IMPORTANT:  You must signal to the OS that your task is complete.
      BackgroundFetch.finish(taskId);
    }

    // Timeout callback is executed when your Task has exceeded its allowed running-time.
    // You must stop what you're doing immediately BackgroundFetch.finish(taskId)
    const onTimeout = async (taskId) => {
      console.warn('[BackgroundFetch] TIMEOUT task: ', taskId);
      BackgroundFetch.finish(taskId);
    }

    // Initialize BackgroundFetch only once when component mounts.
    let status = await BackgroundFetch.configure({
      // minimumFetchInterval: 1,  
      stopOnTerminate: false,
      startOnBoot: true,
      enableHeadless: true
    }, onEvent, onTimeout);

    console.log('[BackgroundFetch] configure status: ', status);
  }


  useEffect(() => {
    setInterval(() => {
      Tracking();
    }, 10000);
    console.log(1111111, 'init')
    initBackgroundFetch();
  }, []);

  return (
    <SafeAreaView style={backgroundStyle}>
      <StatusBar
        barStyle={isDarkMode ? 'light-content' : 'dark-content'}
        backgroundColor={backgroundStyle.backgroundColor}
      />
      <ScrollView
        contentInsetAdjustmentBehavior="automatic"
        style={backgroundStyle}>
        <Header />
        <View
          style={{
            backgroundColor: isDarkMode ? Colors.black : Colors.white,
          }}>
          <Section title="Step One">
            Edit <Text style={styles.highlight}>App.js</Text> to change this
            screen and then come back to see your edits.
          </Section>
          <Section title="See Your Changes">
            <ReloadInstructions />
          </Section>
          <Section title="Debug">
            <DebugInstructions />
          </Section>
          <Section title="Learn More">
            Read the docs to discover what to do next:
          </Section>
          <LearnMoreLinks />
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  sectionContainer: {
    marginTop: 32,
    paddingHorizontal: 24,
  },
  sectionTitle: {
    fontSize: 24,
    fontWeight: '600',
  },
  sectionDescription: {
    marginTop: 8,
    fontSize: 18,
    fontWeight: '400',
  },
  highlight: {
    fontWeight: '700',
  },
});

export default App;
